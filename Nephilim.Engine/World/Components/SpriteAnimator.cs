using Nephilim.Engine.Rendering;
using Nephilim.Engine.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Nephilim.Engine.World.Components
{
    [Serializable]
    public class SpriteAnimator : IComponent, ISerializable
    {
        private string _originalAnimation = string.Empty;
        private SpriteSheet _sheet = null;
        private int _beginIndex = 0, _endIndex = 0;
        private int _currentFrame = 0;
        private bool _loop = true;
        Dictionary<string, AnimationFrame> _animations;
        public string CurrentAnimation { get; private set; }
        public float FrameRate { get; } = 12f;
        public float TimeCache { get; set; } = 0;
        public int CurrentFrame { get => _currentFrame; }

        public bool IsAnimationFinished { get => !_loop && _currentFrame == _endIndex; }
        public SpriteAnimator()
        {

        }

        public void AddToCurrentFrame(int numFrames)
        {
            var value = _currentFrame + numFrames;
            if (value > _endIndex)
            {
                if(_loop)
                    _currentFrame = _beginIndex;
                else if (!string.IsNullOrEmpty(_originalAnimation))
                {
                    SetAnimation(_originalAnimation);
                    _originalAnimation = string.Empty;
                }
            }
            else if (value <= _endIndex)
            {
                _currentFrame = value;
            }
        }

        public SpriteAnimator(Texture texture, int width, int height)
        {
            _sheet = new SpriteSheet(texture, width, width);
        }

        public Vector4 TextureOffset 
        { 
            get
            {
                return _sheet.GetFrameOffset(CurrentFrame);
            } 
        }

        public Texture Texture 
        {
            get 
            {
                return _sheet.Texture;
            } 
        }

        public void PlayOverlayAnimation(string name)
        {
            if (_originalAnimation == name || CurrentAnimation == name)
                return;
            _originalAnimation = CurrentAnimation;
            SetAnimation(name, false);
        }

        public void SetAnimation(string name, bool loop = true)
        {
            if (CurrentAnimation == name)
                return;
            _loop = loop;
            if(_animations.TryGetValue(name, out AnimationFrame value))
            {
                _beginIndex = value.Begin;
                _endIndex = value.End;
                _currentFrame = _beginIndex;
                CurrentAnimation = name;
            }
        }

        public void ForceAnimation(string name, bool loop)
        {
            _loop = loop;
            if (_animations.TryGetValue(name, out AnimationFrame value))
            {
                _beginIndex = value.Begin;
                _endIndex = value.End;
                _currentFrame = _beginIndex;
                CurrentAnimation = name;
            }
        }

        public SpriteAnimator(SerializationInfo info, StreamingContext context)
        {
            var name = (string)info.GetValue("AnimationFile", typeof(string));
            var animations = SpriteAnimation.Deserialize(name);
            _animations = animations.Animations;
            _sheet = animations.Sheet;
            FrameRate = (float)info.GetValue("FrameRate", typeof(float));
            string defaultAnim = (string)info.GetValue("DefaultAnimation", typeof(string));
            SetAnimation(defaultAnim);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
