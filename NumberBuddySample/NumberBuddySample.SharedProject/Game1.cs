using FontBuddyLib;
using GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FontBuddySample
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		#region Properties

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		GameClock CurrentTime;

		List<NumberBuddy> buddies = new List<NumberBuddy>();

		KeyboardState prevKeyboard;

		int number = 0;

		#endregion //Properties

		#region Methods

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
			Content.RootDirectory = "Content";

#if ANDROID
			graphics.PreferredBackBufferWidth = 853;
			graphics.PreferredBackBufferHeight = 480;
#else
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
#endif

			CurrentTime = new GameClock();

			var num = new NumberBuddy(number);
			buddies.Add(num);

			num = new NumberBuddy(number);
			num.BouncyFont.ScalePause = 10f;
			buddies.Add(num);

			num = new NumberBuddy(number);
			num.BouncyFont.ScaleTime = 10f;
			buddies.Add(num);

			num = new NumberBuddy(number);
			num.BouncyFont.ScalePause = 0f;
			buddies.Add(num);

			num = new NumberBuddy(number);
			num.BouncyFont.ScaleTime = 0f;
			buddies.Add(num);

			num = new NumberBuddy(number);
			num.BouncyFont.KillTime = 0f;
			buddies.Add(num);

			num = new NumberBuddy(number);
			num.BouncyFont.ScaleAtEnd = 1.2f;
			buddies.Add(num);

			num = new NumberBuddy(number);
			num.BouncyFont.KillTime = 10f;
			buddies.Add(num);

			num = new NumberBuddy(number);
			num.BouncyFont.ScaleAtEnd = 1.4f;
			num.BouncyFont.ScalePause = 0f;
			buddies.Add(num);
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			foreach (IFontBuddy myBuddy in buddies)
			{
				myBuddy.LoadContent(Content, "ariblk", true, 64);
			}
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
				Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
#if !__IOS__
				Exit();
#endif
			}

			CurrentTime.Update(gameTime);
			var curKeyboard = Keyboard.GetState();
			if (CheckKey(curKeyboard, Keys.A))
			{
				++number;
			}
			else if (CheckKey(curKeyboard, Keys.Z))
			{
				--number;
			}
			else if (CheckKey(curKeyboard, Keys.S))
			{
				number += 100;
			}
			else if (CheckKey(curKeyboard, Keys.X))
			{
				number -= -100;
			}
			else if (CheckKey(curKeyboard, Keys.D))
			{
				number += 1000;
			}
			else if (CheckKey(curKeyboard, Keys.C))
			{
				number -= -1000;
			}
			prevKeyboard = curKeyboard;

			foreach (var num in buddies)
			{
				num.Number = number;
			}

			// TODO: Add your update logic here
			base.Update(gameTime);
		}

		private bool CheckKey(KeyboardState curKeyboard, Keys key)
		{
			return (curKeyboard.IsKeyDown(key) && prevKeyboard.IsKeyUp(key));
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			//get the start point
			Rectangle screen = graphics.GraphicsDevice.Viewport.TitleSafeArea;
			Vector2 position = new Vector2(screen.Left + 32, screen.Top + 32);

			//draw all those fonts
			foreach (IFontBuddy myBuddy in buddies)
			{
				//draw the left justified text
				myBuddy.Write(string.Empty, position, Justify.Left, 1.0f, Color.White, spriteBatch, CurrentTime);

				//draw the centered text
				position.X = screen.Center.X;
				myBuddy.Write(string.Empty, position, Justify.Center, 1.0f, Color.White, spriteBatch, CurrentTime);

				//draw the right justified text
				position.X = screen.Right - 32f;
				myBuddy.Write(string.Empty, position, Justify.Right, 1.0f, Color.White, spriteBatch, CurrentTime);

				//move to the start point for the next font
				position.X = 32f;
				position.Y += myBuddy.MeasureString("test").Y;
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}

		#endregion //Methods
	}
}

