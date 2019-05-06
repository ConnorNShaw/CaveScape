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
        SpriteFont font;

        Player player;

        Texture2D texture;
        KeyboardState old;
        Texture2D playerImage;

        //Holds level and level sections
        Level level;
        List<Section> levelSections;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1800;
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
            //player = new Player(new Rectangle(100, 400, 50, 50));
            levelSections = new List<Section>();
            old = Keyboard.GetState();
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
            playerImage = this.Content.Load<Texture2D>("circle");
            font = Content.Load<SpriteFont>("SpriteFont1");
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

            // Allows the game to exit
            KeyboardState kb = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                this.Exit();
            // TODO: Add your update logic here
            if (kb != old && kb.IsKeyDown(Keys.N))
            {
                level.moveToNextSection();

            }

            if (kb != old && kb.IsKeyDown(Keys.R) && levelSections[level.tracker].player.lives <= 0)
            {
                reset();
            }


            old = kb;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SandyBrown);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //spriteBatch.Draw(playerImage, player.playerLocat, Color.White);
            level.drawLevel(spriteBatch);
            if (levelSections[level.tracker].player.lives <= 0)
            {
                spriteBatch.DrawString(font, "Game Over!", new Vector2(700, 400), Color.Red);
                spriteBatch.DrawString(font, "Press R to restart", new Vector2(500, 700), Color.Red);
            }
            if (level.tracker == levelSections.Count - 1)
            {
                spriteBatch.DrawString(font, "Congratulations!", new Vector2(600, 100), Color.Red);
                spriteBatch.DrawString(font, "You Escaped!", new Vector2(650, 300), Color.Red);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }


        public void reset()
        {
            Initialize();
            LoadContent();
        }



        //Reads a txt file to create game levels
        private void ReadFileAsString(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    lab: while (!reader.EndOfStream)
                    {
                        int levelWidth = Int32.Parse(reader.ReadLine());
                        int levelHeight = Int32.Parse(reader.ReadLine());
                        int numBats = Int32.Parse(reader.ReadLine());
                        int numSpiders = Int32.Parse(reader.ReadLine());

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
                                Section section = new Section(tempArray, levelWidth, levelHeight, texture, numBats, numSpiders);
                                levelSections.Add(section);
                                Console.WriteLine("ADDED A SECTION");
                                tempArray = new string[levelHeight, levelWidth];
                                goto lab;
                            }
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
            for (int i = 0; i < levelSections.Count; i++)
            {
                levelSections[i].setParentLevel(level);
            }
            Console.WriteLine(levelSections.Count);
        }
    }
}
