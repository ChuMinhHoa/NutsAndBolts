
namespace UIAnimation {
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public static class UIAnimationController {
        static Vector3 vectorDefault = Vector3.one;
        static Vector3 vectorRotate3 = new Vector3(0, 0, 3);
        static Vector3 vectorRotate0 = Vector3.zero;
        static Vector3 vectorScaleTo09 = new Vector3(.9f, .9f, .9f);
        static Vector3 vectorScaleTo08 = new Vector3(.8f, .8f, .8f);
        static Vector3 vectorScaleTo11 = new Vector3(1.1f, 1.1f, 1.1f);
        static Vector3 vectorScaleTo12 = new Vector3(1.2f, 1.2f, 1.2f);

        public static void BtnAnimType1(Transform trsDoAnim, float duration, UnityAction actioncallBack = null) {
            Sequence mainSquence = DOTween.Sequence();
            Sequence scaleSequence = DOTween.Sequence();
            scaleSequence.Append(trsDoAnim.DOScale(vectorScaleTo09, duration / 2));
            scaleSequence.Join(trsDoAnim.DORotate(-vectorRotate3, duration / 2));
            scaleSequence.Append(trsDoAnim.DOScale(vectorDefault, duration / 2));
            scaleSequence.Join(trsDoAnim.DORotate(vectorRotate0, duration / 2));

            mainSquence.Append(scaleSequence);
            mainSquence.Play();
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
        }

        public static void BtnAnimZoomBasic(Transform trsDoAnim, float duration, UnityAction actioncallBack = null) {
            Sequence mainSquence = DOTween.Sequence();
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo09, duration / 2));
            mainSquence.Append(trsDoAnim.DOScale(vectorDefault, duration / 2));
            mainSquence.Play();
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
        }

        public static void PanelPopUpBasic(Transform trsDoAnim, float duration,bool loop, UnityAction actioncallBack = null) {
            trsDoAnim.localScale = vectorScaleTo08;
            Sequence mainSquence = DOTween.Sequence();
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo11, duration / 2));
            mainSquence.Append(trsDoAnim.DOScale(vectorDefault, duration / 2));
            mainSquence.Play();
            mainSquence.SetLoops(loop ? -1 : 0);
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
        }

        public static void PopupBigZoom(Transform trsDoAnim, float duration, bool loop, UnityAction actioncallBack = null)
        {
            trsDoAnim.localScale = vectorScaleTo08;
            Sequence mainSquence = DOTween.Sequence();
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo12, duration / 2));
            mainSquence.Append(trsDoAnim.DOScale(vectorDefault, duration / 2));
            mainSquence.Play();
            mainSquence.SetLoops(loop ? -1 : 0);
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
        }

        public static void PopupBigZoomLoop(Transform trsDoAnim, float duration, UnityAction actioncallBack = null)
        {
            Sequence mainSquence = DOTween.Sequence();
            mainSquence.Append(trsDoAnim.DOScale(vectorScaleTo12, duration / 2));
            mainSquence.Append(trsDoAnim.DOScale(vectorDefault, duration / 2));
            mainSquence.Play();
            mainSquence.SetLoops(-1);
            mainSquence.OnComplete(() =>
            {
                if (actioncallBack != null)
                    actioncallBack();
            });
        }
    }
}
