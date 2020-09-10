// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using System;

namespace StatisticalLearning.Math.MatrixDecompositions
{
    /// <summary>
    /// Documentation : https://www.nlafet.eu/wp-content/uploads/2016/01/Deliverable2.6-180427-rev.pdf
    /// </summary>
    public class EigenvalueDecomposition
    {
        private EigenvalueDecomposition(Matrix v, Vector d)
        {
            EigenVectors = v;
            EigenValues = d;
        }

        public Matrix EigenVectors { get; private set; }
        public Vector EigenValues { get; private set; }

        public static EigenvalueDecomposition Decompose(Matrix matrix, bool isSymmetric = false)
        {
            if (isSymmetric)
            {
                throw new NotImplementedException();
            }

            int n = matrix.NbColumns;
            var d = new double[n];
            var V = Matrix.BuildEmptyMatrix(n, n).DoubleValues;
            var H = ((Matrix)matrix.Clone()).DoubleValues;
            var ort = new double[n];
            Orthes(H, V, ort, n);
            HQR2(H, V, d, n);
            return new EigenvalueDecomposition(V, d);
            /*
            var hessenberg = HessenbergDecomposition.Decompose(matrix);
            var result = SchurDecomposition.Decompose(hessenberg.H);
            return new EigenvalueDecomposition(result.EigenVectors, result.EigenValues);
            */
        }

        /// <summary>
        // Nonsymmetric reduction to Hessenberg form.
        // This is derived from the Algol procedures orthes and ortran, by Martin and Wilkinson, 
        // Handbook for Auto. Comp., Vol.ii-Linear Algebra, and the corresponding Fortran subroutines in EISPACK.
        /// </summary>
        private static void Orthes(double[][] H, double[][] V, double[] ort, int n)
        {
            int low = 0;
            int high = n - 1;

            for (int m = low + 1; m <= high - 1; m++)
            {
                // Scale column.

                Double scale = 0;
                for (int i = m; i <= high; i++)
                    scale = scale + System.Math.Abs(H[i][m - 1]);

                if (scale != 0)
                {
                    // Compute Householder transformation.
                    Double h = 0;
                    for (int i = high; i >= m; i--)
                    {
                        ort[i] = H[i][m - 1] / scale;
                        h += ort[i] * ort[i];
                    }

                    Double g = (Double)System.Math.Sqrt(h);
                    if (ort[m] > 0) g = -g;

                    h = h - ort[m] * g;
                    ort[m] = ort[m] - g;

                    // Apply Householder similarity transformation
                    // H = (I - u * u' / h) * H * (I - u * u') / h)
                    for (int j = m; j < n; j++)
                    {
                        Double f = 0;
                        for (int i = high; i >= m; i--)
                            f += ort[i] * H[i][j];

                        f = f / h;
                        for (int i = m; i <= high; i++)
                            H[i][j] -= f * ort[i];
                    }

                    for (int i = 0; i <= high; i++)
                    {
                        Double f = 0;
                        for (int j = high; j >= m; j--)
                            f += ort[j] * H[i][j];

                        f = f / h;
                        for (int j = m; j <= high; j++)
                            H[i][j] -= f * ort[j];
                    }

                    ort[m] = scale * ort[m];
                    H[m][m - 1] = scale * g;
                }
            }

            // Accumulate transformations (Algol's ortran).
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    V[i][j] = (i == j ? 1 : 0);

            for (int m = high - 1; m >= low + 1; m--)
            {
                if (H[m][m - 1] != 0)
                {
                    for (int i = m + 1; i <= high; i++)
                        ort[i] = H[i][m - 1];

                    for (int j = m; j <= high; j++)
                    {
                        Double g = 0;
                        for (int i = m; i <= high; i++)
                            g += ort[i] * V[i][j];

                        // Double division avoids possible underflow.
                        g = (g / ort[m]) / H[m][m - 1];
                        for (int i = m; i <= high; i++)
                            V[i][j] += g * ort[i];
                    }
                }
            }
        }

        /// <summary>
        /// Complex scalar division.
        /// </summary>
        /// <param name="xr"></param>
        /// <param name="xi"></param>
        /// <param name="yr"></param>
        /// <param name="yi"></param>
        /// <param name="cdivr"></param>
        /// <param name="cdivi"></param>
        private static void CDIV(Double xr, Double xi, Double yr, Double yi, out Double cdivr, out Double cdivi)
        {
            Double r;
            Double d;
            if (System.Math.Abs(yr) > System.Math.Abs(yi))
            {
                r = yi / yr;
                d = yr + r * yi;
                cdivr = (xr + r * xi) / d;
                cdivi = (xi - r * xr) / d;
            }
            else
            {
                r = yr / yi;
                d = yi + r * yr;
                cdivr = (r * xr + xi) / d;
                cdivi = (r * xi - xr) / d;
            }
        }

        /// <summary>
        // Nonsymmetric reduction from Hessenberg to real Schur form.   
        // This is derived from the Algol procedure hqr2, by Martin and Wilkinson, Handbook for Auto. Comp.,
        // Vol.ii-Linear Algebra, and the corresponding  Fortran subroutine in EISPACK.
        /// </summary>
        private static void HQR2(double[][] H, double[][] V, double[] d, int nn)
        {
            var e = new double[nn];
            int n = nn - 1;
            int low = 0;
            int high = nn - 1;
            double eps = 2 * 1.11022302462515654042e-16;
            double exshift = 0;
            double p = 0;
            double q = 0;
            double r = 0;
            double s = 0;
            double z = 0;
            double t;
            double w;
            double x;
            double y;

            // Store roots isolated by balanc and compute matrix norm
            Double norm = 0;
            for (int i = 0; i < nn; i++)
            {
                if (i < low | i > high)
                {
                    d[i] = H[i][i];
                    e[i] = 0;
                }

                for (int j = System.Math.Max(i - 1, 0); j < nn; j++)
                    norm = norm + System.Math.Abs(H[i][j]);
            }

            // Outer loop over eigenvalue index
            int iter = 0;
            while (n >= low)
            {
                // Look for single small sub-diagonal element
                int l = n;
                while (l > low)
                {
                    s = System.Math.Abs(H[l - 1][l - 1]) + System.Math.Abs(H[l][l]);

                    if (s == 0)
                        s = norm;

                    if (Double.IsNaN(s))
                        break;

                    if (System.Math.Abs(H[l][l - 1]) < eps * s)
                        break;

                    l--;
                }

                // Check for convergence
                if (l == n)
                {
                    // One root found
                    H[n][n] = H[n][n] + exshift;
                    d[n] = H[n][n];
                    e[n] = 0;
                    n--;
                    iter = 0;
                }
                else if (l == n - 1)
                {
                    // Two roots found
                    w = H[n][n - 1] * H[n - 1][n];
                    p = (H[n - 1][n - 1] - H[n][n]) / 2;
                    q = p * p + w;
                    z = (Double)System.Math.Sqrt(System.Math.Abs(q));
                    H[n][n] = H[n][n] + exshift;
                    H[n - 1][n - 1] = H[n - 1][n - 1] + exshift;
                    x = H[n][n];

                    if (q >= 0)
                    {
                        // Real pair
                        z = (p >= 0) ? (p + z) : (p - z);
                        d[n - 1] = x + z;
                        d[n] = d[n - 1];
                        if (z != 0)
                            d[n] = x - w / z;
                        e[n - 1] = 0;
                        e[n] = 0;
                        x = H[n][n - 1];
                        s = System.Math.Abs(x) + System.Math.Abs(z);
                        p = x / s;
                        q = z / s;
                        r = (Double)System.Math.Sqrt(p * p + q * q);
                        p = p / r;
                        q = q / r;

                        // Row modification
                        for (int j = n - 1; j < nn; j++)
                        {
                            z = H[n - 1][j];
                            H[n - 1][j] = q * z + p * H[n][j];
                            H[n][j] = q * H[n][j] - p * z;
                        }

                        // Column modification
                        for (int i = 0; i <= n; i++)
                        {
                            z = H[i][n - 1];
                            H[i][n - 1] = q * z + p * H[i][n];
                            H[i][n] = q * H[i][n] - p * z;
                        }

                        // Accumulate transformations
                        for (int i = low; i <= high; i++)
                        {
                            z = V[i][n - 1];
                            V[i][n - 1] = q * z + p * V[i][n];
                            V[i][n] = q * V[i][n] - p * z;
                        }
                    }
                    else
                    {
                        // Complex pair
                        d[n - 1] = x + p;
                        d[n] = x + p;
                        e[n - 1] = z;
                        e[n] = -z;
                    }

                    n = n - 2;
                    iter = 0;
                }
                else
                {
                    // No convergence yet     

                    // Form shift
                    x = H[n][n];
                    y = 0;
                    w = 0;
                    if (l < n)
                    {
                        y = H[n - 1][n - 1];
                        w = H[n][n - 1] * H[n - 1][n];
                    }

                    // Wilkinson's original ad hoc shift
                    if (iter == 10)
                    {
                        exshift += x;
                        for (int i = low; i <= n; i++)
                            H[i][i] -= x;

                        s = System.Math.Abs(H[n][n - 1]) + System.Math.Abs(H[n - 1][n - 2]);
                        x = y = (Double)0.75 * s;
                        w = (Double)(-0.4375) * s * s;
                    }

                    // MATLAB's new ad hoc shift
                    if (iter == 30)
                    {
                        s = (y - x) / 2;
                        s = s * s + w;
                        if (s > 0)
                        {
                            s = (Double)System.Math.Sqrt(s);
                            if (y < x) s = -s;
                            s = x - w / ((y - x) / 2 + s);
                            for (int i = low; i <= n; i++)
                                H[i][i] -= s;
                            exshift += s;
                            x = y = w = (Double)0.964;
                        }
                    }

                    iter = iter + 1;

                    // Look for two consecutive small sub-diagonal elements
                    int m = n - 2;
                    while (m >= l)
                    {
                        z = H[m][m];
                        r = x - z;
                        s = y - z;
                        p = (r * s - w) / H[m + 1][m] + H[m][m + 1];
                        q = H[m + 1][m + 1] - z - r - s;
                        r = H[m + 2][m + 1];
                        s = System.Math.Abs(p) + System.Math.Abs(q) + System.Math.Abs(r);
                        p = p / s;
                        q = q / s;
                        r = r / s;
                        if (m == l)
                            break;
                        if (System.Math.Abs(H[m][m - 1]) * (System.Math.Abs(q) + System.Math.Abs(r)) < eps * (System.Math.Abs(p) * (System.Math.Abs(H[m - 1][m - 1]) + System.Math.Abs(z) + System.Math.Abs(H[m + 1][m + 1]))))
                            break;
                        m--;
                    }

                    for (int i = m + 2; i <= n; i++)
                    {
                        H[i][i - 2] = 0;
                        if (i > m + 2)
                            H[i][i - 3] = 0;
                    }

                    // Double QR step involving rows l:n and columns m:n
                    for (int k = m; k <= n - 1; k++)
                    {
                        bool notlast = (k != n - 1);
                        if (k != m)
                        {
                            p = H[k][k - 1];
                            q = H[k + 1][k - 1];
                            r = (notlast ? H[k + 2][k - 1] : 0);
                            x = System.Math.Abs(p) + System.Math.Abs(q) + System.Math.Abs(r);
                            if (x != 0)
                            {
                                p = p / x;
                                q = q / x;
                                r = r / x;
                            }
                        }

                        if (x == 0) break;

                        s = (Double)System.Math.Sqrt(p * p + q * q + r * r);
                        if (p < 0) s = -s;

                        if (s != 0)
                        {
                            if (k != m)
                                H[k][k - 1] = -s * x;
                            else
                                if (l != m)
                                H[k][k - 1] = -H[k][k - 1];

                            p = p + s;
                            x = p / s;
                            y = q / s;
                            z = r / s;
                            q = q / p;
                            r = r / p;

                            // Row modification
                            for (int j = k; j < nn; j++)
                            {
                                p = H[k][j] + q * H[k + 1][j];
                                if (notlast)
                                {
                                    p = p + r * H[k + 2][j];
                                    H[k + 2][j] = H[k + 2][j] - p * z;
                                }

                                H[k][j] = H[k][j] - p * x;
                                H[k + 1][j] = H[k + 1][j] - p * y;
                            }

                            // Column modification
                            for (int i = 0; i <= System.Math.Min(n, k + 3); i++)
                            {
                                p = x * H[i][k] + y * H[i][k + 1];
                                if (notlast)
                                {
                                    p = p + z * H[i][k + 2];
                                    H[i][k + 2] = H[i][k + 2] - p * r;
                                }

                                H[i][k] = H[i][k] - p;
                                H[i][k + 1] = H[i][k + 1] - p * q;
                            }

                            // Accumulate transformations
                            for (int i = low; i <= high; i++)
                            {
                                p = x * V[i][k] + y * V[i][k + 1];
                                if (notlast)
                                {
                                    p = p + z * V[i][k + 2];
                                    V[i][k + 2] = V[i][k + 2] - p * r;
                                }

                                V[i][k] = V[i][k] - p;
                                V[i][k + 1] = V[i][k + 1] - p * q;
                            }
                        }
                    }
                }
            }

            // Backsubstitute to find vectors of upper triangular form
            if (norm == 0)
            {
                return;
            }

            for (n = nn - 1; n >= 0; n--)
            {
                p = d[n];
                q = e[n];

                // Real vector
                if (q == 0)
                {
                    int l = n;
                    H[n][n] = 1;
                    for (int i = n - 1; i >= 0; i--)
                    {
                        w = H[i][i] - p;
                        r = 0;
                        for (int j = l; j <= n; j++)
                            r = r + H[i][j] * H[j][n];

                        if (e[i] < 0)
                        {
                            z = w;
                            s = r;
                        }
                        else
                        {
                            l = i;
                            if (e[i] == 0)
                            {
                                H[i][n] = (w != 0) ? (-r / w) : (-r / (eps * norm));
                            }
                            else
                            {
                                // Solve real equations
                                x = H[i][i + 1];
                                y = H[i + 1][i];
                                q = (d[i] - p) * (d[i] - p) + e[i] * e[i];
                                t = (x * s - z * r) / q;
                                H[i][n] = t;
                                H[i + 1][n] = (System.Math.Abs(x) > System.Math.Abs(z)) ? ((-r - w * t) / x) : ((-s - y * t) / z);
                            }

                            // Overflow control
                            t = System.Math.Abs(H[i][n]);
                            if ((eps * t) * t > 1)
                                for (int j = i; j <= n; j++)
                                    H[j][n] = H[j][n] / t;
                        }
                    }
                }
                else if (q < 0)
                {
                    // Complex vector
                    int l = n - 1;

                    // Last vector component imaginary so matrix is triangular
                    if (System.Math.Abs(H[n][n - 1]) > System.Math.Abs(H[n - 1][n]))
                    {
                        H[n - 1][n - 1] = q / H[n][n - 1];
                        H[n - 1][n] = -(H[n][n] - p) / H[n][n - 1];
                    }
                    else
                    {
                        CDIV(0, -H[n - 1][n], H[n - 1][n - 1] - p, q, out H[n - 1][n - 1], out H[n - 1][n]);
                    }

                    H[n][n - 1] = 0;
                    H[n][n] = 1;
                    for (int i = n - 2; i >= 0; i--)
                    {
                        Double ra, sa, vr, vi;
                        ra = 0;
                        sa = 0;
                        for (int j = l; j <= n; j++)
                        {
                            ra = ra + H[i][j] * H[j][n - 1];
                            sa = sa + H[i][j] * H[j][n];
                        }

                        w = H[i][i] - p;

                        if (e[i] < 0)
                        {
                            z = w;
                            r = ra;
                            s = sa;
                        }
                        else
                        {
                            l = i;
                            if (e[i] == 0)
                            {
                                CDIV(-ra, -sa, w, q, out H[i][n - 1], out H[i][n]);
                            }
                            else
                            {
                                // Solve complex equations
                                x = H[i][i + 1];
                                y = H[i + 1][i];
                                vr = (d[i] - p) * (d[i] - p) + e[i] * e[i] - q * q;
                                vi = (d[i] - p) * 2 * q;
                                if (vr == 0 & vi == 0)
                                    vr = eps * norm * (System.Math.Abs(w) + System.Math.Abs(q) + System.Math.Abs(x) + System.Math.Abs(y) + System.Math.Abs(z));
                                CDIV(x * r - z * ra + q * sa, x * s - z * sa - q * ra, vr, vi, out H[i][n - 1], out H[i][n]);
                                if (System.Math.Abs(x) > (System.Math.Abs(z) + System.Math.Abs(q)))
                                {
                                    H[i + 1][n - 1] = (-ra - w * H[i][n - 1] + q * H[i][n]) / x;
                                    H[i + 1][n] = (-sa - w * H[i][n] - q * H[i][n - 1]) / x;
                                }
                                else
                                {
                                    CDIV(-r - y * H[i][n - 1], -s - y * H[i][n], z, q, out H[i + 1][n - 1], out H[i + 1][n]);
                                }
                            }

                            // Overflow control
                            t = System.Math.Max(System.Math.Abs(H[i][n - 1]), System.Math.Abs(H[i][n]));
                            if ((eps * t) * t > 1)
                            {
                                for (int j = i; j <= n; j++)
                                {
                                    H[j][n - 1] = H[j][n - 1] / t;
                                    H[j][n] = H[j][n] / t;
                                }
                            }
                        }
                    }
                }
            }

            // Vectors of isolated roots
            for (int i = 0; i < nn; i++)
                if (i < low | i > high)
                    for (int j = i; j < nn; j++)
                        V[i][j] = H[i][j];

            // Back transformation to get eigenvectors of original matrix
            for (int j = nn - 1; j >= low; j--)
            {
                for (int i = low; i <= high; i++)
                {
                    z = 0;
                    for (int k = low; k <= System.Math.Min(j, high); k++)
                        z = z + V[i][k] * H[k][j];
                    V[i][j] = z;
                }
            }
        }
    }
}
