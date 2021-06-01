namespace Asteroids3D
{
    public class CanvasSpace
    {
        // Size of the canvas is stored in a static variable,
        // letting other codes to run with less dependencies.
        // This class is the only place where you can change canvas size.
        private static int canvasHalfSize = 8192;

        public static int CanvasHalfSize()
        {
            return canvasHalfSize;
        }
    }
}