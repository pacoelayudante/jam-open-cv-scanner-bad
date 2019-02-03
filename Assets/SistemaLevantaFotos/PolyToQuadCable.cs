using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class PolyToQuadCable : MatCable
{
    public float areaEsperada = 0.2f;

    override public void Procesar(MatCable matCable){
        if (!matCable || matCable.SalidaPts == null || matCable.Salida == null) return;
        salida = matCable.Salida;
        puntos = new Point[0][];
        
        // find best guess for our contour
        List<Point[]> goodCandidates = new List<Point[]>(matCable.SalidaPts);
        goodCandidates.RemoveAt(0);
        Point[] paperContour = GetBestMatchingContour(salida.Width * salida.Height, goodCandidates, matCable.SalidaPts[0]);
        if (null == paperContour)
        {
            puntos = goodCandidates.ToArray();
            return;
        }

        // exact hit - we have 4 corners
        if (paperContour.Length == 4)
        {
            paperContour = SortCorners(paperContour);
        }
        // some hit: we either have 3 points or > 4 which we can try to make a 4-corner shape
        else if (paperContour.Length > 2)
        {
            // yet contour might contain too much points: along with calculation inaccuracies we might face a
            // bended piece of paper, missing corner etc.
            // the solution is to use bounding box
            RotatedRect bounds = Cv2.MinAreaRect(paperContour);
            Point2f[] points = bounds.Points();
            Point[] intPoints = System.Array.ConvertAll(points, p => new Point(System.Math.Round(p.X), System.Math.Round(p.Y)));
            Point[] fourCorners = SortCorners(intPoints);

            // array.ClosestElement is not efficient but we can live with it since it's quite few
            // elements to search for
            System.Func<Point, Point, double> distance = (Point x, Point y) => Point.Distance(x, y);
            Point[] closest = new Point[4];
            for (int i = 0; i < fourCorners.Length; ++i)
                closest[i] = paperContour.ClosestElement(fourCorners[i], distance);

            paperContour = closest;
        }

        if(paperContour!=null)puntos = new Point[][]{ paperContour };
    }

    /// <summary>
    /// Takes candidate shape and combined hull and returns best match
    /// </summary>
    /// <param name="areaSize">Area size</param>
    /// <param name="candidates">Candidates</param>
    /// <param name="hull">Hull</param>
    /// <returns></returns>
    private Point[] GetBestMatchingContour(double areaSize, List<Point[]> candidates, Point[] hull)
    {
        Point[] result = hull;
        if (candidates.Count == 1)
            result = candidates[0];
        else if (candidates.Count > 1)
        {
            List<Point> keys = new List<Point>();
            foreach (var c in candidates)
                keys.AddRange(c);

            Point[] joinedCandidates = Cv2.ConvexHull(keys);
            Point[] joinedHull = Cv2.ApproxPolyDP(joinedCandidates, Cv2.ArcLength(joinedCandidates, true) * 0.01, true);
            result = joinedHull;
        }

        // check further
        // if (Settings.DropBadGuess)
        {
            double area = Cv2.ContourArea(result);
            if (area / areaSize < areaEsperada * 0.75)
                result = null;
        }

        return result;
    }

    /// <summary>
    /// Sorts corners as { left-top, right-top, right-bottom, left-bottom }
    /// </summary>
    /// <param name="corners">Input points</param>
    /// <returns>Sorted corners</returns>
    private Point[] SortCorners(Point[] corners)
    {
        if (corners.Length != 4)
            throw new OpenCvSharpException("\"corners\" must be an array of 4 elements");

        // divide vertically
        System.Array.Sort<Point>(corners, (a, b) => a.Y.CompareTo(b.Y));
        Point[] tops = new Point[] { corners[0], corners[1] }, bottoms = new Point[] { corners[2], corners[3] };

        // divide horizontally
        System.Array.Sort<Point>(corners, (a, b) => a.X.CompareTo(b.X));
        Point[] lefts = new Point[] { corners[0], corners[1] }, rights = new Point[] { corners[2], corners[3] };

        // fetch final array
        Point[] output = new Point[] {
                tops[0],
                tops[1],
                bottoms[0],
                bottoms[1]
            };
        if (!lefts.Contains(tops[0]))
            output.Swap(0, 1);
        if (!rights.Contains(bottoms[0]))
            output.Swap(2, 3);

        // done
        return output;
    }
}
