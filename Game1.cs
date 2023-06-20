using aimm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.NetworkInformation;
using System.Reflection;

namespace aimm
{
    public class Game1 : Game
    {
        private enum GameState
        {
            MainMenu,
            Playing
        }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Camera camera;

        private Model targetModel;
        private Matrix[] targetTransforms;
        private Vector3 targetPosition;

        private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);

        private GameState gameState;
        private MainMenu mainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            // camera set up
            camera = new Camera(this);
            camera.SetPosition(new Vector3(0, 0, 10));
            Components.Add(camera);

            gameState = GameState.MainMenu;
            mainMenu = new MainMenu();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            targetModel = Content.Load<Model>("sphere");
            //targetModel = Content.Load<Model>("a/models/trees_pine");

            // initial target position
            targetPosition = Vector3.Zero;
        }

        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    mainMenu.Update(gameTime);

                    if (mainMenu.StartGame)
                    {
                        gameState = GameState.Playing;
                    }

                    break;

                case GameState.Playing:
                    float movementSpeed = 0.1f;
                    Vector3 moveVector = Vector3.Zero;

                    KeyboardState keyboardState = Keyboard.GetState();
                    if (keyboardState.IsKeyDown(Keys.W))
                        moveVector.Z = -movementSpeed;
                    if (keyboardState.IsKeyDown(Keys.S))
                        moveVector.Z = movementSpeed;
                    if (keyboardState.IsKeyDown(Keys.A))
                        moveVector.X = -movementSpeed;
                    if (keyboardState.IsKeyDown(Keys.D))
                        moveVector.X = movementSpeed;

                    Vector3 newPosition = camera.PreviewMove(moveVector);
                    camera.Move(newPosition);

                    targetPosition.X += 0.01f;
                    targetPosition.Y += 0.005f;

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            

            switch (gameState)
            {
                case GameState.MainMenu:
               
                    mainMenu.Draw(spriteBatch);
                    break;

                case GameState.Playing:
                    //target model
                    DrawModel(targetModel, world, view, projection);

                    foreach (ModelMesh mesh in targetModel.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.World = targetTransforms[mesh.ParentBone.Index] * Matrix.CreateTranslation(targetPosition);
                            effect.View = camera.View;
                            effect.Projection = camera.Projection;
                        }

                        mesh.Draw();
                    }

                    break;
            }

            base.Draw(gameTime);
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }

}
    
     