using TMPro;
using UnityEngine;

#pragma warning disable 0649

namespace ArenaShooter.Controllers
{

    class UIWaveController : Controller<UIWaveController>
    {

        #region References

        [Header("UI Refereneces")]
        [SerializeField] private Animator waveTextAnimator;
        [SerializeField] private TMP_Text waveText;

        #endregion

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

    }

}
