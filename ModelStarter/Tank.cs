using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStarter
{
    


    /// <summary>
    /// A class representing a tank in the game
    /// </summary>
    public class Tank : IFollowable
    {

        // The game this tank belongs to 
        Game game;

        // The tank's model
        Model model;

        /// <summary>
        /// The position of the tank in the world 
        /// </summary>
        public Vector3 Position => position;

        /// <summary>
        /// The angle the tank is facing (in radians)
        /// </summary>
        public float Facing => facing;

        // The tank's position in the world 
        Vector3 position = Vector3.Zero;

        // The direction the tank is facing
        float facing = 0;

        /// <summary>
        /// Constructs a new Tank instance
        /// </summary>
        /// <param name="game">The game this tank belongs to</param>
        public Tank(Game game)
        {
            this.game = game;
            model = game.Content.Load<Model>("tank");
            transforms = new Matrix[model.Bones.Count];
            //set turret fields
            turretBone = model.Bones["turret_geo"];
            turretTransform = turretBone.Transform;

            // Set the canon fields
            canonBone = model.Bones["canon_geo"];
            canonTransform = canonBone.Transform;

        }

        /// <summary>
        /// Gets or sets the IHeightMap this tank is driving upon
        /// </summary>
        public IHeightMap HeightMap { get; set; }


        /// <summary>
        /// Gets or sets the speed of the tank
        /// </summary>
        public float Speed { get; set; } = 0.5f;
        public float rotationSpeed { get; set; } = 0.1f;

        // The bone transformation matrices of the tank
        Matrix[] transforms;

        //turret rotation fields
        ModelBone turretBone;
        Matrix turretTransform;
        float turretRotation = 0;

        // Barrel fields 
        ModelBone canonBone;
        Matrix canonTransform;
        float canonRotation = 0;

        Vector3 IFollowable.Position { get => position; set => position = Position; }
        float IFollowable.Facing { get => facing; set => facing = Facing; }

        /// <summary>
        /// Updates the tank, moving it based on player input
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            var direction = Vector3.Transform(Vector3.Forward, Matrix.CreateRotationY(facing));

            if (keyboard.IsKeyDown(Keys.W))
            {
                position -= Speed * direction;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                position += (Speed / 2) * direction;
            }

            if (keyboard.IsKeyDown(Keys.A))
            {
                facing += rotationSpeed;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                facing -= rotationSpeed;
            }

            // rotate the turret
            if (keyboard.IsKeyDown(Keys.Left))
            {
                turretRotation += rotationSpeed;
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                turretRotation -= rotationSpeed;
            }

            // Update the canon angle
            if (keyboard.IsKeyDown(Keys.Up))
            {
                canonRotation -= rotationSpeed;
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                canonRotation += rotationSpeed;
            }
            // Limit canon rotation to a reasonable range 
            canonRotation = MathHelper.Clamp(canonRotation, -MathHelper.PiOver4, 0);


            // Set the tank's height based on the HeightMap
            if (HeightMap != null)
            {
                position.Y = HeightMap.GetHeightAt(position.X, position.Z);
            }

        }

        /// <summary>
        /// Draws the tank in the world
        /// </summary>
        /// <param name="camera">The camera used to render the world</param>
        public void Draw(ICamera camera)
        {
            Matrix world = Matrix.CreateRotationY(facing) * Matrix.CreateTranslation(position);

            Matrix view = camera.View;

            Matrix projection = camera.Projection;
            model.CopyAbsoluteBoneTransformsTo(transforms);
            // draw the tank meshes 
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                }
                mesh.Draw();
            }
            // apply turret rotation
            turretBone.Transform = Matrix.CreateRotationY(turretRotation) * turretTransform;
            //apply cannon movement
            canonBone.Transform = Matrix.CreateRotationX(canonRotation) * canonTransform;


            //model.Draw(world, view, projection);
        }


    }




}
