using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace CaveScape
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;

        Texture2D texture;

        //Holds level and level sections
        Level level;
        List<Section> levelSections;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player(new Rectangle(100, 400, 50, 50), Content.Load<Texture2D>("circle"));
            levelSections = new List<Section>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = this.Content.Load<Texture2D>("White Square");
            ReadFileAsString(@"Content/tutorial levels.txt");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            player.playerControls(ks);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(player.playerSprite, player.playerLocat, Color.White);
            //level.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Reads a txt file to create game levels
        private void ReadFileAsString(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    int levelWidth = Int32.Parse(reader.ReadLine());
                    int levelHeight = Int32.Parse(reader.ReadLine());

                    string[,] tempArray = new string[levelHeight, levelWidth];
                    int r = 0;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!line.Equals("►")) //alt + 16 to get "►"
                        {
                            string[] characters = line.Split(',');
                            for (int c = 0; c < characters.Length; c++)
                            {
                                tempArray[r, c] = characters[c];
                            }
                            r++;
                        }
                        else
                        {
                            Section section = new Section(tempArray, levelWidth, levelHeight, texture);
                            levelSections.Add(section);
                            Console.WriteLine("Created \n A \n Section");
                            tempArray = new string[levelHeight, levelWidth];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            level = new Level(levelSections);
        }
    }
}
