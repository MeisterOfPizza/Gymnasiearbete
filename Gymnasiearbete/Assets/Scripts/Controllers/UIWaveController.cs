using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIWaveController : Controller<UIWaveController>
    {

        #region References

        [Header("UI Refereneces")]
        [SerializeField] private Image waveProgressBar;

        [Space]
        [SerializeField] private Animator waveTextAnimator;
        [SerializeField] private TMP_Text waveText;

        [Header("Values")]
        [SerializeField] private float waveProgressBarFillSpeed = 10f;

        #endregion

        #region Private variables

        private float targetWaveProgressBarFillAmount;

        #endregion

        private void Update()
        {
            waveProgressBar.fillAmount = Mathf.Lerp(waveProgressBar.fillAmount, targetWaveProgressBarFillAmount, waveProgressBarFillSpeed * Time.deltaTime);
        }

        public void WaveStartEvent(WaveStartEvent @event)
        {
            waveTextAnimator.SetTrigger("Appear");

            waveText.text = $"Wave {@event.WaveNumber} containing {@event.EnemyCount} hostiles begins in...";
        }

        public void WaveEndEvent(WaveEndEvent @event)
        {
            if (!waveTextAnimator.GetCurrentAnimatorStateInfo(0).IsName("Appear"))
            {
                waveTextAnimator.SetTrigger("Appear");
            }

            waveText.text = $"Wave {@event.WaveNumber} ended";
        }

        public void WaveCountdownEvent(WaveCountdownEvent @event)
        {
            waveText.text = @event.Time == 0 ? "NOW" : @event.Time.ToString();

            if (@event.Time == 0)
            {
                waveTextAnimator.Play("Disappear");
            }
            else
            {
                waveTextAnimator.Play("Pulse");
            }
        }

        public void WaveNumberEvent(WaveNumberEvent @event)
        {
            waveText.text = "Wave " + @event.Wave;

            if (!waveTextAnimator.GetCurrentAnimatorStateInfo(0).IsName("Appear"))
            {
                waveTextAnimator.SetTrigger("Appear");
            }
        }

        public void WaveProgressEvent(WaveProgressEvent @event)
        {
            targetWaveProgressBarFillAmount = @event.Progress;
        }

    }

}
