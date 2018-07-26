using System;
using Windows.UI.Composition;

namespace Days.Helper
{
    public class Animation
    {
        public static ScalarKeyFrameAnimation CreateBlurIncreaseAnimation(Compositor compositor)
        {
            ScalarKeyFrameAnimation shadowBlurAnimation = compositor.CreateScalarKeyFrameAnimation();
            var linear = compositor.CreateLinearEasingFunction();
            shadowBlurAnimation.InsertKeyFrame(0.5f, 40f, linear);
            shadowBlurAnimation.Duration = TimeSpan.FromSeconds(0.5);

            return shadowBlurAnimation;
        }

        public static ScalarKeyFrameAnimation CreateBlurDecreaseAnimation(Compositor compositor)
        {
            ScalarKeyFrameAnimation shadowBlurAnimation = compositor.CreateScalarKeyFrameAnimation();
            var linear = compositor.CreateLinearEasingFunction();
            shadowBlurAnimation.InsertKeyFrame(0.5f, 10f, linear);
            shadowBlurAnimation.Duration = TimeSpan.FromSeconds(0.5);

            return shadowBlurAnimation;
        }
    }
}
