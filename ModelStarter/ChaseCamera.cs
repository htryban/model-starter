using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStarter
{
    class ChaseCamera : ICamera
    {
        Game game;

        Matrix projection;

        Matrix view;

        /// <summary>
        /// The target this camera should follow
        /// </summary>
        public IFollowable Target { get; set; }

        /// <summary>
        /// The positon of the camera in relation to its target
        /// </summary>
        public Vector3 Offset { get; set; }

        /// <summary>
        /// The camera's view matrix
        /// </summary>
        public Matrix View => view;

        /// <summary>
        /// The camera's projection matrix
        /// </summary>
        public Matrix Projection => projection;

        /// <summary>
        /// Creates a new ChaseCamera
        /// </summary>
        /// <param name="game">The game this camera belongs to</param>
        /// <param name="offset">The offset the camera should maintian from its target</param>
        public ChaseCamera(Game game, Vector3 offset)
        {
            this.game = game;
            this.Offset = offset;
            this.projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                game.GraphicsDevice.Viewport.AspectRatio,
                1,
                1000
            );
            this.view = Matrix.CreateLookAt(
                Vector3.Zero,
                offset,
                Vector3.Up
            );
        }

        /// <summary>
        /// Updates the camera, placing it relative to the target
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            if (Target == null) return;

            // calculate the position of the camera
            var position = Target.Position + Vector3.Transform(Offset, Matrix.CreateRotationY(Target.Facing));

            this.view = Matrix.CreateLookAt(
                position,
                Target.Position,
                Vector3.Up
            );
        }



    }
}
