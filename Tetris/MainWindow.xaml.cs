using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Tetris
{

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		const long PointsPerCompletedRow = 50;
		const long NumberOfTicksToIncreaseGameSpeedBy = 1000000;
		const long InitialNumberOfTimerTicks = 10000000;

		static DispatcherTimer timer = new DispatcherTimer();
		static bool[,] containsElementMatrix;
		static Random randomNumberGenerator = new Random();
		static long currentScore = 0L;

		Shape currentShape;
		Shape nextShape;

		bool isNewShapeCreated = false;
		bool isGameLost = false;
		bool isPaused = false;

		int offset;
		int midpoint;

		int level;

		private Shape GetRandomShape()
		{
			int numberOfDifferentShapes = 7;
			int randomNonNegativeInteger = randomNumberGenerator.Next();
			int index = randomNonNegativeInteger % numberOfDifferentShapes;
			
			//ShapeElement[] shit = null;
			Shape shape = null;
			switch (index)
			{
				case 0:
					/*
					 * x 
					 * x
					 * x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint, 1), new ShapeElement(midpoint, 2), new ShapeElement(midpoint + 1, 2) };
					shape = new LBlock(midpoint);
					break;
				case 1:
					/*
					 * x x x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint + 1, 0), new ShapeElement(midpoint + 2, 0), new ShapeElement(midpoint + 3, 0) };
					shape = new LineBlock(midpoint);
					break;
				case 2:
					/*
					 *   x
					 * x x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 1), new ShapeElement(midpoint + 1, 1), new ShapeElement(midpoint + 1, 0), new ShapeElement(midpoint + 2, 0) };
					shape = new TBlock(midpoint);
					break;
				case 3:
					/*
					 *   x x
					 * x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 1), new ShapeElement(midpoint + 1, 1), new ShapeElement(midpoint + 1, 0), new ShapeElement(midpoint + 2, 1) };
					shape = new SBlock(midpoint);
					break;
				case 4:
					/*
					 * x x
					 * x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint + 1, 0), new ShapeElement(midpoint, 1), new ShapeElement(midpoint + 1, 1) };
					shape = new SquareBlock(midpoint);
					break;
				case 5:
					/*
					 * x x
					 *   x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint + 1, 0), new ShapeElement(midpoint + 1, 1), new ShapeElement(midpoint + 2, 1) };
					shape = new ReverseSBlock(midpoint);
					break;
				case 6:
					/*   x
					 *   x
					 * x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint, 1), new ShapeElement(midpoint, 2), new ShapeElement(midpoint - 1, 2) };
					shape = new JBlock(midpoint);
					break;
				default:
					break;
			}
			return shape;
		}

		public MainWindow()
		{
			InitializeComponent();
			InitializeGameComponents();
			
		}

		private void InitializeGameComponents()
		{
			InitializeTimer();
			offset = 2;
			level = 0;
			levelBox.Text = level.ToString();
			midpoint = (gameField.ColumnDefinitions.Count / 2) - offset;
			containsElementMatrix = new bool[gameField.RowDefinitions.Count, gameField.ColumnDefinitions.Count];
			currentShape = GetRandomShape();
			nextShape = GetRandomShape();
			ShowNextShapeInPreviewGrid(nextShape);
			DrawShape(currentShape);
			//playButton.Click += new RoutedEventHandler(playButton_Click);
		}

		private void InitializeTimer()
		{
			timer.Interval = new TimeSpan(InitialNumberOfTimerTicks);
			timer.Tick += new EventHandler(OnTimerTick);
			timer.Start();
		}

		void OnTimerTick(object sender, EventArgs e)
		{
			MoveDown();
		}

		void DrawShape(Shape shape)
		{
			foreach (ShapeElement point in shape.ShapeElements)
			{
				Label shapeElement = new Label();
				shapeElement.Background = Brushes.Red;
				shapeElement.BorderBrush = Brushes.White;
				shapeElement.BorderThickness = new Thickness(1);
				gameField.Children.Add(shapeElement);
				Grid.SetColumn(shapeElement, point.X);
				Grid.SetRow(shapeElement, point.Y);
			}
		}

		void LoseGame()
		{
			timer.Stop();
			MessageBoxResult userChoice = MessageBox.Show("Play again?", "Game Over!", MessageBoxButton.YesNoCancel, MessageBoxImage.None);
			if (userChoice == MessageBoxResult.Yes)
			{
				ResetValues();
			}
			else
			{
				isGameLost = true;
			}
		}

		void ResetValues()
		{
			containsElementMatrix = new bool[stackedShapes.RowDefinitions.Count, stackedShapes.ColumnDefinitions.Count];
			stackedShapes.Children.Clear();
			timer.Interval = new TimeSpan(InitialNumberOfTimerTicks);
			gameField.Children.Clear();
			currentShape = GetRandomShape();
			scoreBox.Text = "0";
			levelBox.Text = "0";
			currentScore = 0;
			level = 0;
			DrawShape(currentShape);
			timer.Start();
		}

		private void MoveRight()
		{
			if (currentShape.ShapeElements.All(element => element.X + 1 < gameField.ColumnDefinitions.Count && containsElementMatrix[element.Y, element.X + 1] == false))
			{
				foreach (ShapeElement point in currentShape.ShapeElements)
				{
					point.X++;
				}
			}
			gameField.Children.Clear();
			DrawShape(currentShape);
		}

		void MoveDown()
		{
			if (currentShape.ShapeElements.All(element => element.Y + 1 < gameField.RowDefinitions.Count && containsElementMatrix[element.Y + 1, element.X] == false))
			{			
				gameField.Children.Clear();
				foreach (ShapeElement point in currentShape.ShapeElements)
				{
					point.Y++;
				}
			}
			else if (currentShape.ShapeElements.Any(element => element.Y == 0 && containsElementMatrix[element.Y + 1, element.X]))
			{
				LoseGame();
				return;
			}
			else
			{
				
				isNewShapeCreated = true;
				AddShapeToStackGrid();
				currentShape = nextShape;
				nextShape = GetRandomShape();
				ShowNextShapeInPreviewGrid(nextShape);
				//DrawShape(currentShape);
				CheckForCompleteRow();
			}

			DrawShape(currentShape);

		}

		private void ShowNextShapeInPreviewGrid(Shape nextShape)
		{
			previewGrid.Children.Clear();
			foreach (ShapeElement point in nextShape.ShapeElements)
			{
				Label shapeElement = new Label();
				shapeElement.Background = Brushes.Gray;
				shapeElement.BorderThickness = new Thickness(1);
				shapeElement.BorderBrush = Brushes.White;
				previewGrid.Children.Add(shapeElement);
				Grid.SetColumn(shapeElement, point.X - midpoint + 1);
				Grid.SetRow(shapeElement, point.Y + 1);
			}
		}


		private void mainWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.LeftCtrl)
			{

				if (isPaused == false)
				{
					timer.Stop();
					isPaused = true;
					return;
				}
				else
				{
					isPaused = false;
					timer.Start();
				}
			}
			if (isGameLost == false && isPaused == false)
			{
				switch (e.Key)
				{
					case Key.Left:
						MoveLeft();
						break;
					case Key.Right:
						MoveRight();
						break;
					case Key.Down:
						MoveDown();
						break;
					case Key.Up:
						Rotate(currentShape);
						break;
					case Key.Space:
						while (isNewShapeCreated == false && isGameLost == false)
						{
							MoveDown();
						}
						isNewShapeCreated = false;
						break;
					default:
						break;
				}
			}		
		}

	

		private void AddShapeToStackGrid()
		{
			gameField.Children.Clear();
			foreach (ShapeElement pt in currentShape.ShapeElements)
			{
				containsElementMatrix[pt.Y, pt.X] = true;
				Label shapeElement = new Label();
				shapeElement.Background = Brushes.Gray;
				shapeElement.BorderThickness = new Thickness(1);
				shapeElement.BorderBrush = Brushes.White;
				stackedShapes.Children.Add(shapeElement);
				Grid.SetColumn(shapeElement, pt.X);
				Grid.SetRow(shapeElement, pt.Y);
			}
		}

		private void MoveLeft()
		{
			if (currentShape.ShapeElements.All(element => (element.X - 1 >= 0 && containsElementMatrix[element.Y, element.X - 1] == false)))
			{
				foreach (ShapeElement point in currentShape.ShapeElements)
				{
					point.X--;
				}
			}
			gameField.Children.Clear();
			DrawShape(currentShape);
		}

		private void Rotate(Shape shape)
		{
			ShapeElement pivotElement = new ShapeElement(shape.ShapeElements[1].X, shape.ShapeElements[1].Y);
			if (shape.ShapeElements.All(element => pivotElement.X + pivotElement.Y - element.X >= 0 && pivotElement.X + pivotElement.Y - element.X < gameField.RowDefinitions.Count
				&& pivotElement.X - pivotElement.Y + element.Y >= 0 && pivotElement.X - pivotElement.Y + element.Y < gameField.ColumnDefinitions.Count
				&& containsElementMatrix[pivotElement.X + pivotElement.Y - element.X, pivotElement.X - pivotElement.Y + element.Y] == false))
			{
				foreach (ShapeElement point in shape.ShapeElements)
				{

					int x = point.X;
					point.X = pivotElement.X - pivotElement.Y + point.Y;
					point.Y = pivotElement.X + pivotElement.Y - x;
				}
				gameField.Children.Clear();
				DrawShape(shape);
			}
		}

		private void CheckForCompleteRow()
		{
			int completedRowIndex = -1;
			for (int row = containsElementMatrix.GetLength(0) - 1; row >= 0; row--)
			{
				bool isRowComplete = true;
				for (int col = 0; col < containsElementMatrix.GetLength(1); col++)
				{
					if (containsElementMatrix[row, col] == false)
					{
						isRowComplete = false;
						break;
					}

				}

				if (isRowComplete == true)
				{
					completedRowIndex = row;
					break;
				}
			}

			if (completedRowIndex != -1)
			{
				RemoveCompleteRow(completedRowIndex);
				CheckForCompleteRow();
			}
		}

		private void IncreaseSpeed()
		{
			if (currentScore % 500 == 0)
			{
				level++;
				levelBox.Text = level.ToString();
				long currentTimerTicks = timer.Interval.Ticks;
				currentTimerTicks -= NumberOfTicksToIncreaseGameSpeedBy;
				if (currentTimerTicks == 0)
				{
					LoseGame();
				}
				timer.Interval = new TimeSpan(currentTimerTicks);
			}
		}

		private void RemoveCompleteRow(int indexOfCompleteRow)
		{
			
			for (int row = indexOfCompleteRow; row > 0; row--)
			{
				for (int col = 0; col < containsElementMatrix.GetLength(1); col++)
				{
					containsElementMatrix[row, col] = containsElementMatrix[row - 1, col];
				}
			}
			currentScore += PointsPerCompletedRow;
			scoreBox.Text = currentScore.ToString();
			stackedShapes.Children.Clear();
			for (int row = containsElementMatrix.GetLength(0) - 1; row > 0; row--)
			{
				for (int col = 0; col < containsElementMatrix.GetLength(1); col++)
				{
					if (containsElementMatrix[row, col] == true)
					{
						Label shapeElement = new Label();
						shapeElement.Background = Brushes.Gray;
						shapeElement.BorderThickness = new Thickness(1);
						shapeElement.BorderBrush = Brushes.White;
						stackedShapes.Children.Add(shapeElement);
						Grid.SetColumn(shapeElement, col);
						Grid.SetRow(shapeElement, row);
					}
				}
			}
			IncreaseSpeed();
		}
	}
}
