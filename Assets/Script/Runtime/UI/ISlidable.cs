using System;
using DG.Tweening;

namespace SuperUltra.Container
{
    
    public interface ISlidable
    {
        Tween SlideOut(float animationTime = 0.2f);
        Tween SlideIn(float animationTime = 0.5f);
        void ChangeSlideDirection(SlideDirection direction);
    }

}