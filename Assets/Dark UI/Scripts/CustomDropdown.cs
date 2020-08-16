﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Michsky.UI.Dark
{
    public class CustomDropdown : MonoBehaviour, IPointerExitHandler
    {
        [Header("OBJECTS")]
        public GameObject triggerObject;
        public TextMeshProUGUI selectedText;
        public Image selectedImage;
        public Transform itemParent;
        public GameObject itemObject;
        public GameObject scrollbar;
        public Transform listParent;
        private Transform currentListParent;
        private VerticalLayoutGroup itemList;

        [Header("SETTINGS")]
        public bool enableIcon = true;
        public bool enableTrigger = true;
        public bool enableScrollbar = true;
        public bool setHighPriorty = true;
        public bool outOnPointerExit;
        public bool isListItem;
        public bool invokeAtStart = false;

        [Space(10)]
        [SerializeField]
        public List<Item> dropdownItems = new List<Item>();
        public int selectedItemIndex = 0;
        [Space(10)]

        private Animator dropdownAnimator;
        private TextMeshProUGUI setItemText;
        private Image setItemImage;

        Sprite imageHelper;
        string textHelper;
        bool isOn;

        public enum AnimationType
        {
            FADING,
            SLIDING,
            STYLISH
        }

        [System.Serializable]
        public class Item
        {
            public string itemName = "Dropdown Item";
            public Sprite itemIcon;
            public UnityEvent OnItemSelection;
        }

        void Start()
        {
            dropdownAnimator = this.GetComponent<Animator>();
            itemList = itemParent.GetComponent<VerticalLayoutGroup>();

            foreach (Transform child in itemParent)
            {
                GameObject.Destroy(child.gameObject);
            }

            int index = 0;
            for (int i = 0; i < dropdownItems.Count; ++i)
            {
                GameObject go = Instantiate(itemObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(itemParent, false);

                setItemText = go.GetComponentInChildren<TextMeshProUGUI>();
                textHelper = dropdownItems[i].itemName;
                setItemText.text = textHelper;

                Transform goImage;
                goImage = go.gameObject.transform.Find("Icon");
                setItemImage = goImage.GetComponent<Image>();
                imageHelper = dropdownItems[i].itemIcon;
                setItemImage.sprite = imageHelper;

                Button itemButton;
                itemButton = go.GetComponent<Button>();
                itemButton.onClick.AddListener(dropdownItems[i].OnItemSelection.Invoke);
                itemButton.onClick.AddListener(Animate);
                itemButton.onClick.AddListener(delegate
                {
                    ChangeDropdownInfo(index = go.transform.GetSiblingIndex());
                });

                if (invokeAtStart == true)
                {
                    dropdownItems[i].OnItemSelection.Invoke();
                }
            }

            selectedText.text = dropdownItems[selectedItemIndex].itemName;
            selectedImage.sprite = dropdownItems[selectedItemIndex].itemIcon;

            if (enableScrollbar == true)
            {
                itemList.padding.right = 25;
                scrollbar.SetActive(true);
            }

            else
            {
                itemList.padding.right = 8;
                Destroy(scrollbar);
            }

            if (enableIcon == false)
                selectedImage.gameObject.SetActive(false);

            else
                selectedImage.gameObject.SetActive(true);

            currentListParent = transform.parent;
        }

        public void ChangeDropdownInfo(int itemIndex)
        {
            selectedImage.sprite = dropdownItems[itemIndex].itemIcon;
            selectedText.text = dropdownItems[itemIndex].itemName;
            selectedItemIndex = itemIndex;
            // dropdownItems[itemIndex].OnItemSelection.Invoke();
        }

        public void Animate()
        {
            if (isOn == false)
            {
                dropdownAnimator.Play("Dropdown In");
                isOn = true;

                if (isListItem == true)
                    gameObject.transform.SetParent(listParent, true);
            }

            else if (isOn == true)
            {
                dropdownAnimator.Play("Dropdown Out");
                isOn = false;

                if (isListItem == true)
                    gameObject.transform.SetParent(currentListParent, true);
            }

            if (enableTrigger == true && isOn == false)
                triggerObject.SetActive(false);

            else if (enableTrigger == true && isOn == true)
                triggerObject.SetActive(true);

            if (setHighPriorty == true)
                gameObject.transform.SetAsLastSibling();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (outOnPointerExit == true)
            {
                if (isOn == true)
                {
                    dropdownAnimator.Play("Dropdown Out");
                    isOn = false;
                }
                triggerObject.SetActive(false);

                if (isListItem == true)
                    gameObject.transform.SetParent(currentListParent, true);
            }
        }
    }
}