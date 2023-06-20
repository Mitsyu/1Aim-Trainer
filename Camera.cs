using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aimm
{
    class Camera : GameComponent
    {
        //point the camera is looking at, set to origin - Vector3.zero?
        private Vector3 cameraLookAt;

        //camera position in 3D area
        public Vector3 Position { get; private set; }

        //camera rotation around the X Y Z axes
        public Vector3 Rotation { get; private set; }

        //projection matrix of camera(transforms 3d points into 2d space)
        public Matrix Projection { get; protected set; }

        //represents the view matrix of the camera
        public Matrix View { get { return Matrix.CreateLookAt(Position, cameraLookAt, Vector3.Up); } }
        //calculates the view matrix camera orientation and position in the 3D space
        public Camera(Game game) : base(game)
        {
            this.cameraLookAt = Vector3.Zero;
            this.Position = Vector3.Zero;
            this.Rotation = Vector3.Zero;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, Game.GraphicsDevice.Viewport.AspectRatio,
                0.05f, 1000.0f);
        }

        private void UpdateLookAt()
        {//updates the cameraLookAt point based on the cameras position and rotation
            Matrix rotationMatrix = Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y);
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);

            cameraLookAt = Position + lookAtOffset;
        }

        public Vector3 PreviewMove(Vector3 movement)
        {//calculates the cameras new position if movement is applied
            Matrix rotate = Matrix.CreateRotationY(Rotation.Y);

            movement = Vector3.Transform(movement, rotate);

            return Position + movement;
        }

        public void Move(Vector3 position)
        {// updates the camera position and calls UpdateLookAt to update the cameraLookAt point 
            Position = position;

            UpdateLookAt();
        }

        public void SetPosition(Vector3 position)
        {//sets the camera's position to the specified position
            Vector3 cameraView = PreviewMove(position);

            Move(cameraView);
        }

        public void SetRotation(Vector3 rotation)
        {
            this.Rotation = rotation;
            //calls
            UpdateLookAt();//to update the cameraLookAt point based on the new rotation
        }//sets the cameras rotation
    }
    
    
}
