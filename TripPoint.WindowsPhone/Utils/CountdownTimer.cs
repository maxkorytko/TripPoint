using System;
using System.Windows.Threading;

namespace TripPoint.WindowsPhone.Utils
{
    public class CountdownTimer
    {
        private DispatcherTimer _timer;
        private EventHandler _timerTickEventHandler;

        public delegate void Explode();

        public CountdownTimer()
        {
            _timer = new DispatcherTimer();

            _timerTickEventHandler = (sender, args) =>
            {
                Stop();

                if (OnExplode != null)
                    OnExplode();
            };
        }

        public TimeSpan Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        public Explode OnExplode { get; set; }

        public void Start()
        {
            _timer.Tick += _timerTickEventHandler;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Tick -= _timerTickEventHandler;
        }
    }
}
