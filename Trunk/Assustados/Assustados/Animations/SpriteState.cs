using System;

namespace Assustados.Animations
{
    /// <summary>
    /// Estados que o sprite pode assumir
    /// </summary>
    public enum SpriteState
    {
        IdleUp,
        IdleDown,
        IdleLeft,
        IdleRight,
        WalkUp,
        WalkDown,
        WalkLeft,
        WalkRight, 
        Attack,
        Die
    }
}
