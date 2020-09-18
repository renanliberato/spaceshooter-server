using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SpaceshooterServer.Models
{
    public class MatchState
    {
        public PlayerState PlayerState { get; set; }

        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }

        public MatchState()
        {
            MinX = 0;
            MinY = 0;
            MaxX = 1000;
            MaxY = 1000;
            PlayerState = new PlayerState(this)
            {
                Health = 10,
                MaxHealth = 10,
                X = 200,
                Y = 200
            };
        }

        public void Tick()
        {
            PlayerState.Update();
        }
    }

    public class PlayerState
    {
        private MatchState matchState;

        public PlayerState(MatchState matchState)
        {
            this.matchState = matchState;
            this.Dx = 0;
            this.Dy = 0;
            this.rotateSpeed = 4;
            this.accelerationForce = 0.03f;
            this.dashForce = 6f;
            this.dashDecreaseForce = 0.11f;
            this.dashDecreaseTreshold = 0.5f;
        }

        public float Angle { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Dx { get; set; }
        public float Dy { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public bool acceleratingBackwards { get; private set; }
        public bool acceleratingFrontwards { get; private set; }
        public float accelerationForce { get; private set; }
        public float dashForce { get; private set; }
        public bool rotatingLeft { get; private set; }
        public float rotateSpeed { get; private set; }
        public bool rotatingRight { get; private set; }
        public bool dashingRight { get; private set; }
        public float dashDecreaseTreshold { get; private set; }
        public float dashDecreaseForce { get; private set; }
        public bool dashingLeft { get; private set; }

        public void Update()
        {
            this.MoveShip();
        }

        public void MoveShip()
        {
            //if (!this.acceleratingFrontwards && !this.acceleratingBackwards && Math.Abs(this.Dy) <= 0.1)
            //{
            //    this.Dy = 0;
            //}

            this.AccelerateFrontwards();
            this.DeaccelerateFrontwards();

            this.AccelerateBackwards();
            this.DeaccelerateBackwards();

            if (this.rotatingLeft)
            {
                this.RotateAngle(this.rotateSpeed * -1);
            }

            if (this.rotatingRight)
            {
                this.RotateAngle(this.rotateSpeed);
            }

            if (this.dashingRight)
            {
                if (this.Dx > this.dashDecreaseTreshold)
                {
                    this.Dx -= this.dashDecreaseForce;
                }
                else
                {
                    this.Dx = 0;
                    this.dashingRight = false;
                }
            }

            if (this.dashingLeft)
            {
                if (this.Dx < this.dashDecreaseTreshold)
                {
                    this.Dx += this.dashDecreaseForce;
                }
                else
                {
                    this.Dx = 0;
                    this.dashingLeft = false;
                }
            }

            this.MoveAccordingToAngle("front", this.Angle, this.Dy);
            this.MoveAccordingToAngle("left", this.Angle, this.Dx);
        }

        public void RotateAngle(float dAngle)
        {
            this.Angle += dAngle;
        }

        public void RotateToAngle(float angle)
        {
            this.Angle = angle;
        }

        public void AccelerateFrontwards()
        {
            this.Dy = Math.Min(5, this.Dy + (this.acceleratingFrontwards ? 1 : 0) * this.accelerationForce);
        }

        public void AccelerateBackwards()
        {
            this.Dy = Math.Max(-5, this.Dy - (this.acceleratingBackwards ? 1 : 0) * this.accelerationForce);
        }

        public void DeaccelerateFrontwards()
        {
            if (!this.acceleratingFrontwards && this.Dy > 0)
                this.Dy -= this.accelerationForce;
        }

        public void DeaccelerateBackwards()
        {
            if (!this.acceleratingBackwards && this.Dy < 0)
                this.Dy += this.accelerationForce;
        }

        public void Move(float x, float y)
        {
            X = Math.Max(matchState.MinX, Math.Min(matchState.MaxX, X + x));
            Y = Math.Max(matchState.MinY, Math.Min(matchState.MaxY, Y + y));
        }

        public void MoveAccordingToAngle(string direction, float angle, float speed)
        {
            switch (direction)
            {
                case "front":
                    this.Move(
                        speed * (float)Math.Sin(angle * Math.PI / 180),
                        speed * (float)Math.Cos(angle * Math.PI / 180) * -1
                    );
                    break;
                case "back":
                    this.Move(
                        speed * (float)Math.Cos(angle * Math.PI / 180),
                        speed * (float)Math.Sin(angle * Math.PI / 180)
                    );
                    break;
                case "left":
                    this.Move(
                        speed * (float)Math.Cos(angle * Math.PI / 180),
                        speed * (float)Math.Sin(angle * Math.PI / 180)
                    );
                    break;
                case "right":
                    this.Move(
                        speed * (float)Math.Cos(angle * Math.PI / 180) * -1,
                        speed * (float)Math.Sin(angle * Math.PI / 180) * -1
                    );
                    break;
            }
        }

        internal void UpdateCommands(bool acceleratingFrontwards, bool acceleratingBackwards, bool rotatingLeft, bool rotatingRight)
        {
            this.acceleratingFrontwards = acceleratingFrontwards;
            this.acceleratingBackwards = acceleratingBackwards;
            this.rotatingLeft = rotatingLeft;
            this.rotatingRight = rotatingRight;
        }

        public void DashLeft()
        {
            this.dashingLeft = true;
            this.Dx = this.dashForce * -1;
        }

        public void DashRight()
        {
            this.dashingRight = true;
            this.Dx = this.dashForce;
        }

        internal void UpdatePosition(float x, float y, float angle)
        {
            throw new NotImplementedException();
        }
    }
}
