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
            shadowBlurAnimation.InsertKeyFrame(0.0f, 10f, linear);
            shadowBlurAnimation.InsertKeyFrame(0.4f, 25f, linear);
            shadowBlurAnimation.InsertKeyFrame(0.8f, 40f, linear);
            shadowBlurAnimation.Duration = TimeSpan.FromSeconds(0.8);

            return shadowBlurAnimation;
        }

        public static ScalarKeyFrameAnimation CreateBlurDecreaseAnimation(Compositor compositor)
        {
            ScalarKeyFrameAnimation shadowBlurAnimation = compositor.CreateScalarKeyFrameAnimation();
            var linear = compositor.CreateLinearEasingFunction();
            shadowBlurAnimation.InsertKeyFrame(0.0f, 40f, linear);
            shadowBlurAnimation.InsertKeyFrame(0.4f, 25f, linear);
            shadowBlurAnimation.InsertKeyFrame(0.8f, 10f, linear);
            shadowBlurAnimation.Duration = TimeSpan.FromSeconds(0.8);

            return shadowBlurAnimation;
        }
    }
}
