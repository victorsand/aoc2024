namespace aoc2024;

public class Day4(int day, int input, bool isTest) : DayBase(day, input, isTest)
{
    protected override int Run1()
    {
        var word = "XMAS";
        var input = GetInput();
        var inputWidth = input.First().Length;
        if (input.Any(i => i.Length != inputWidth)) throw new Exception("Input widths not equal");
        var inputHeight = input.Count;

        var grid = new Grid { XSize = inputWidth, YSize = inputHeight, Data = input};
        var startCoordinate = new Coordinate { X = 0, Y = 0 };

        var numWords = 0;

        for (var i = 0; i < inputWidth; i++)
        {
            for (var j = 0; j < inputHeight; j++)
            {
                numWords += grid.LookForWord(startCoordinate, word);
                startCoordinate.X++;
            }
            startCoordinate.X = 0;
            startCoordinate.Y++;
        }

        return numWords;
    }

    protected override int Run2()
    {
        var word = "MAS";
        var input = GetInput();
        var inputWidth = input.First().Length;
        if (input.Any(i => i.Length != inputWidth)) throw new Exception("Input widths not equal");
        var inputHeight = input.Count;

        var grid = new Grid { XSize = inputWidth, YSize = inputHeight, Data = input};
        var startCoordinate = new Coordinate { X = 0, Y = 0 };

        var numWords = 0;

        for (var i = 0; i < inputWidth; i++)
        {
            for (var j = 0; j < inputHeight; j++)
            {
                numWords += grid.LookForCrossWord(startCoordinate, word);
                startCoordinate.X++;
            }
            startCoordinate.X = 0;
            startCoordinate.Y++;
        }

        return numWords;
    }
    
    private class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    private enum Direction
    {
        Right,
        Left,
        Up,
        Down,
        DiagDownRight,
        DiagUpRight,
        DiagDownLeft,
        DiagUpLeft
    }

    private class Grid
    {
        public int XSize { get; set; }
        public int YSize { get; set; }
        public List<string> Data { get; set; } = new();

        private char Read(Coordinate coordinate)
        {
            return Data[coordinate.Y][coordinate.X];
        }

        public int LookForCrossWord(Coordinate coordinate, string word)
        {
            if (word.Length % 2 == 0) throw new Exception("Length must be odd");
            if (Read(coordinate) != word[word.Length / 2]) return 0;

            if (!TryMoveDiagUpLeft(coordinate, 1, out var topLeftCoord)) return 0;
            if (!TryMoveDiagDownLeft(coordinate, 1, out var downLeftCoord)) return 0;
            if (!TryMoveDiagUpRight(coordinate, 1, out var topRightCoord)) return 0;
            if (!TryMoveDiagDownRight(coordinate, 1, out var downRightCoord)) return 0;

            var downRight = SearchWord(Direction.DiagDownRight, topLeftCoord!, word);
            var upLeft = SearchWord(Direction.DiagUpLeft, downRightCoord!, word);
            if (!(downRight || upLeft)) return 0;
            var downLeft = SearchWord(Direction.DiagDownLeft, topRightCoord!, word);
            var upRight = SearchWord(Direction.DiagUpRight, downLeftCoord!, word);

            return (downLeft || upRight) ? 1 : 0;
        }
        
        public int LookForWord(Coordinate coordinate, string word)
        {
            if (Read(coordinate) != word[0]) return 0;
            
            var searches = new Dictionary<Direction, Coordinate>
            {
                { Direction.Right, coordinate },
                { Direction.Left, coordinate },
                { Direction.Up, coordinate },
                { Direction.Down, coordinate },
                { Direction.DiagDownRight, coordinate },
                { Direction.DiagUpRight, coordinate },
                { Direction.DiagDownLeft, coordinate },
                { Direction.DiagUpLeft, coordinate }
            };

            return searches.Count(search => SearchWord(search.Key, search.Value, word));
        }

        private bool SearchWord(Direction direction, Coordinate coordinate, string word)
        {
            var index = 0;
            var position = coordinate;
            if (word[index] != Read(position)) return false;
            while (index < word.Length - 1)
            {
                index++;
                if (TryMove(position, direction, 1, out var newPosition))
                {
                    position = newPosition!;
                    if (word[index] != Read(position))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }


        private bool TryMove(Coordinate current, Direction direction, int numSteps, out Coordinate? result)
        {
            return direction switch
            {
                Direction.Right => TryMoveRight(current, numSteps, out result),
                Direction.Left => TryMoveLeft(current, numSteps, out result),
                Direction.Up => TryMoveUp(current, numSteps, out result),
                Direction.Down => TryMoveDown(current, numSteps, out result),
                Direction.DiagDownRight => TryMoveDiagDownRight(current, numSteps, out result),
                Direction.DiagUpRight => TryMoveDiagUpRight(current, numSteps, out result),
                Direction.DiagDownLeft => TryMoveDiagDownLeft(current, numSteps, out result),
                Direction.DiagUpLeft => TryMoveDiagUpLeft(current, numSteps, out result),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        private bool TryMoveRight(Coordinate current, int numSteps, out Coordinate? result)
        {
            if (current.X + numSteps >= XSize)
            {
                result = null;
                return false;
            }
            result = new Coordinate { X = current.X + numSteps, Y = current.Y };
            return true;
        }
        
        private bool TryMoveLeft(Coordinate current, int numSteps, out Coordinate? result)
        {
            if (current.X - numSteps < 0)
            {
                result = null;
                return false;
            }
            result = new Coordinate { X = current.X - numSteps, Y = current.Y };
            return true;
        }
        
        private bool TryMoveUp(Coordinate current, int numSteps, out Coordinate? result)
        {
            if (current.Y - numSteps < 0)
            {
                result = null;
                return false;
            }
            result = new Coordinate { X = current.X, Y = current.Y - numSteps };
            return true;
        }
        
        private bool TryMoveDown(Coordinate current, int numSteps, out Coordinate? result)
        {
            if (current.Y + numSteps >= YSize)
            {
                result = null;
                return false;
            }
            result = new Coordinate { X = current.X, Y = current.Y + numSteps };
            return true;
        }

        private bool TryMoveDiagDownRight(Coordinate current, int numSteps, out Coordinate? result)
        {
            if (!TryMoveDown(current, numSteps, out var tempCoord))
            {
                result = null;
                return false;
            }
            if (!TryMoveRight(tempCoord!, numSteps, out var resultCoord))
            {
                result = null;
                return false;
            }
            result = resultCoord;
            return true;
        }
        
        private bool TryMoveDiagDownLeft(Coordinate current, int numSteps, out Coordinate? result)
        {
            if (!TryMoveDown(current, numSteps, out var tempCoord))
            {
                result = null;
                return false;
            }
            if (!TryMoveLeft(tempCoord!, numSteps, out var resultCoord))
            {
                result = null;
                return false;
            }
            result = resultCoord;
            return true;
        }
        
        private bool TryMoveDiagUpLeft(Coordinate current, int numSteps, out Coordinate? result)
        {
            if (!TryMoveUp(current, numSteps, out var tempCoord))
            {
                result = null;
                return false;
            }
            if (!TryMoveLeft(tempCoord!, numSteps, out var resultCoord))
            {
                result = null;
                return false;
            }
            result = resultCoord;
            return true;
        }
        
        private bool TryMoveDiagUpRight(Coordinate current, int numSteps, out Coordinate? result)
        {
            if (!TryMoveUp(current, numSteps, out var tempCoord))
            {
                result = null;
                return false;
            }
            if (!TryMoveRight(tempCoord!, numSteps, out var resultCoord))
            {
                result = null;
                return false;
            }
            result = resultCoord;
            return true;
        }
        
    }
    
}