using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class UiController : MonoBehaviour, IController{
        [SerializeField] private TextMeshProUGUI distanceTxt;
        [SerializeField] private TextMeshProUGUI weatherStatusTxt;
        [SerializeField] private CanvasGroup tutorialPanel;

        public void Init(EarthController controller) {
            controller.CameraController.AddListenerOnChangeDistance(ChangeDistanceText);
            controller.AddListenerOnChangeWeather(ChangeWeatherText);
            tutorialPanel.DOFade(0, 10);
        }

        private void ChangeDistanceText(float distance) {
            distanceTxt.text = $"{distance:N} m";
        }

        private void ChangeWeatherText(Weather weather) {
            weatherStatusTxt.text = weather.ToString();
        }

    }
}