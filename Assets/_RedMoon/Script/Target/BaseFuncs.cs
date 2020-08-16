using System;

namespace Polaris.Base
{
	public class BaseFuncs
	{
		public BaseFuncs ()
		{
		}
		public static int Increase(int val, int step, int max, int min, bool repeatable = false){
			val += step;
			if(val <= max){
				return val;
			}
			if (repeatable == true) {
				val = min;
			} else {
				val = max;
			}
			return val;
		}
		public static int Decrease(int val, int step, int max, int min, bool repeatable = false){
			val -= step;
			if(val >= min){
				return val;
			}
			if (repeatable == true) {
				val = max;
			} else {
				val = min;
			}
			return val;
		}
//		public static long Increase(long val, long step, long max, long min, bool repeatable = false){
//			val += step;
//			if(val <= max){
//				return val;
//			}
//			if (repeatable == true) {
//				val = min;
//			} else {
//				val = max;
//			}
//			return val;
//		}
//		public static long Decrease(long val, long step, long max, long min, bool repeatable = false){
//			val -= step;
//			if(val <= min){
//				return val;
//			}
//			if (repeatable == true) {
//				val = max;
//			} else {
//				val = min;
//			}
//			return val;
//		}
	}
}

