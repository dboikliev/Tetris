using System;
using System.Linq;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Tetris
{
	abstract class Shape : IShape
	{
		private int pivot;

		const byte BorderThickness = 1;

		private ShapeElement[] shapeElements;
        private ShapeElement[] pivotElement;

		//public Shape(int pivot, ShapeElement[] shapeElements)
		//{
		//	this.pivot = pivot;
		//	this.shapeElements = shapeElements;
		//}

		public ShapeElement[] ShapeElements
		{
			get
			{
				return this.shapeElements;
			}
            protected set
            {
                this.shapeElements = value;
            }
		}

		public void MoveDown(Grid gameField, bool[,] containsElementMatrix, ref bool isNewShapeCreated)
		{
			if (this.shapeElements.All(element => element.Y + 1 < gameField.RowDefinitions.Count && containsElementMatrix[element.Y + 1, element.X] == false))
			{
				gameField.Children.Clear();
				foreach (ShapeElement point in this.shapeElements)
				{
					point.Y++;
				}
			}
			else if (this.shapeElements.Any(element => element.Y == 0 && containsElementMatrix[element.Y + 1, element.X]))
			{
				//LoseGame();
				return;
			}
			else
			{

				isNewShapeCreated = true;
				//AddShapeToStackGrid();
				//currentShape = nextShape;
				//nextShape = GetRandomShape();
				//ShowNextShapeInPreviewGrid(nextShape);
				//DrawShape(currentShape);
				//CheckForCompleteRow();
			}

			DrawShape(gameField);

		}

		public void MoveLeft(Grid gameField, bool[,] containsElementMatrix)
		{
			if (this.shapeElements.All(element => (element.X - 1 >= 0 && containsElementMatrix[element.Y, element.X - 1] == false)))
			{
				foreach (ShapeElement point in this.shapeElements)
				{
					point.X--;
				}
			}
			gameField.Children.Clear();
			DrawShape(gameField);
		}

		public void MoveRight(Grid gameField, bool[,] containsElementMatrix)
		{
			if (shapeElements.All(element => element.X + 1 < gameField.ColumnDefinitions.Count && containsElementMatrix[element.Y, element.X + 1] == false))
			{
				foreach (ShapeElement point in this.shapeElements)
				{
					point.X++;
				}
			}
			gameField.Children.Clear();
			DrawShape(gameField);
		}

		public void DrawShape(Grid gameField)
		{
			foreach (ShapeElement point in this.shapeElements)
			{
				Label shapeElement = new Label();
				shapeElement.Background = Brushes.Red;
				shapeElement.BorderBrush = Brushes.White;
				shapeElement.BorderThickness = new Thickness(BorderThickness);
				gameField.Children.Add(shapeElement);
				Grid.SetColumn(shapeElement, point.X);
				Grid.SetRow(shapeElement, point.Y);
			}
		}

		public Shape Create(string shapeName)
		{
			Shape shape = null;
			switch (shapeName)
			{
				case "LBlock":
					/*
					 * x 
					 * x
					 * x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint, 1), new ShapeElement(midpoint, 2), new ShapeElement(midpoint + 1, 2) };
					shape = new LBlock(pivot);
					break;
				case "LineBlock":
					/*
					 * x x x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint + 1, 0), new ShapeElement(midpoint + 2, 0), new ShapeElement(midpoint + 3, 0) };
					shape = new LineBlock(pivot);
					break;
				case "TBlock":
					/*
					 *   x
					 * x x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 1), new ShapeElement(midpoint + 1, 1), new ShapeElement(midpoint + 1, 0), new ShapeElement(midpoint + 2, 0) };
					shape = new TBlock(pivot);
					break;
				case "SBlock":
					/*
					 *   x x
					 * x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 1), new ShapeElement(midpoint + 1, 1), new ShapeElement(midpoint + 1, 0), new ShapeElement(midpoint + 2, 1) };
					shape = new SBlock(pivot);
					break;
				case "SquareBlock":
					/*
					 * x x
					 * x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint + 1, 0), new ShapeElement(midpoint, 1), new ShapeElement(midpoint + 1, 1) };
					shape = new SquareBlock(pivot);
					break;
				case "ReverseSBlock":
					/*
					 * x x
					 *   x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint + 1, 0), new ShapeElement(midpoint + 1, 1), new ShapeElement(midpoint + 2, 1) };
					shape = new ReverseSBlock(pivot);
					break;
				case "JBlock":
					/*   x
					 *   x
					 * x x
					 */
					//shit = new ShapeElement[] { new ShapeElement(midpoint, 0), new ShapeElement(midpoint, 1), new ShapeElement(midpoint, 2), new ShapeElement(midpoint - 1, 2) };
					shape = new JBlock(pivot);
					break;
				default:
                    throw new Exception("No such shape exists");
					break;
			}
			return shape;
		}


        public void MoveLeft()
        {
            foreach (ShapeElement element in this.ShapeElements)
            {
                element.X--;
            }
        }

        public void MoveRight()
        {
            foreach (ShapeElement element in this.ShapeElements)
            {
                element.X++;
            }
        }

        public void MoveDown()
        {
            foreach (ShapeElement element in this.ShapeElements)
            {
                element.Y++;
            }
        }

        public void Rotate()
        {
            foreach (ShapeElement point in this.ShapeElements)
            {
                int x = point.X;
                //point.X = pivotElement.X - pivotElement.Y + point.Y;
                //point.Y = pivotElement.X + pivotElement.Y - x;
            }
        }

    }

	
}
