using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Collections;
using System.Threading;

namespace KryptografiaWizualna
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap bitmap;
        Bitmap Converted_bitmap;
        Zbior Macierze_4subpiksele;
        Zbior Macierze_2subpiksele;
        Bitmap udzial_1;
        Bitmap udzial_2;
        Bitmap Loaded_bitmap_1;
        Bitmap Loaded_bitmap_2;
        Bitmap result;

        public struct Zbior
        {
            public Podzbior Macierze_biale;
            public Podzbior Macierze_czarne;

            public Zbior(Podzbior macierze_biale, Podzbior macierze_czarne)
            {
                Macierze_biale = macierze_biale;
                Macierze_czarne = macierze_czarne;
            }
        }
        public struct Podzbior
        {
            public Macierz[] Macierze;

            public Podzbior(Macierz[] macierze)
            {
                Macierze = macierze;
            }
        }
        public struct Macierz
        {
            public int[] udzial1;
            public int[] udzial2;

            public Macierz(int[] udzial1, int[] udzial2)
            {
                this.udzial1 = udzial1;
                this.udzial2 = udzial2;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Podzbior podzbior = new Podzbior(new Macierz[] { new Macierz(new int[] { 0, 0, 1, 1 }, new int[] { 0, 0, 1, 1 }),
                                                             new Macierz(new int[] { 1, 1, 0, 0 }, new int[] { 1, 1, 0, 0 }),
                                                             new Macierz(new int[] { 0, 1, 0, 1 }, new int[] { 0, 1, 0, 1 }),
                                                             new Macierz(new int[] { 1, 0, 1, 0 }, new int[] { 1, 0, 1, 0 }),
                                                             new Macierz(new int[] { 0, 1, 1, 0 }, new int[] { 0, 1, 1, 0 }),
                                                             new Macierz(new int[] { 1, 0, 0, 1 }, new int[] { 1, 0, 0, 1 })
                                                           });

            Podzbior podzbior2 = new Podzbior(new Macierz[] { new Macierz(new int[] { 1, 0, 0, 1 }, new int[] { 0, 1, 1, 0 }),
                                                              new Macierz(new int[] { 0, 1, 1, 0 }, new int[] { 1, 0, 0, 1 }),
                                                              new Macierz(new int[] { 0, 0, 1, 1 }, new int[] { 1, 1, 0, 0 }),
                                                              new Macierz(new int[] { 1, 1, 0, 0 }, new int[] { 0, 0, 1, 1 }),
                                                              new Macierz(new int[] { 0, 1, 0, 1 }, new int[] { 1, 0, 1, 0 }),
                                                              new Macierz(new int[] { 1, 0, 1, 0 }, new int[] { 0, 1, 0, 1 })
                                                           });   
            Macierze_4subpiksele = new Zbior(podzbior,podzbior2);


            Podzbior podzbior3 = new Podzbior(new Macierz[] { new Macierz(new int[] { 0, 1}, new int[] { 0, 1}),
                                                              new Macierz(new int[] { 1, 0}, new int[] { 1, 0 })
                                                           });

            Podzbior podzbior4 = new Podzbior(new Macierz[] { new Macierz(new int[] { 1, 0}, new int[] { 0, 1}),
                                                              new Macierz(new int[] { 0, 1}, new int[] { 1, 0})
                                                           });

            Macierze_2subpiksele = new Zbior(podzbior3, podzbior4);
        }

        private void Button_Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\Kryptografia Wizualna";
            openFileDialog.Filter = "All files(*.*) |*.*|BMP (*.bmp)|*.bmp| PNG (*.png)|*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                bitmap = new Bitmap(path);
                Converted_bitmap = bitmap;
                Image_Orginal.Source = new BitmapImage(new Uri(path));




                if (!isBlackWhite(bitmap))
                {
                    Slider_Threshold_R.IsEnabled = true;
                    Slider_Threshold_G.IsEnabled = true;
                    Slider_Threshold_B.IsEnabled = true;
                    Button_ShowTresholdResult.IsEnabled = true;
                    Button_enrypt.IsEnabled = false;
                } else
                {
                    Slider_Threshold_R.IsEnabled = false;
                    Slider_Threshold_G.IsEnabled = false;
                    Slider_Threshold_B.IsEnabled = false;
                    Button_ShowTresholdResult.IsEnabled = false;
                    Button_enrypt.IsEnabled = true;
                }
            }
        }

        public void Bitmap_tresholding()
        {
            Bitmap tresholded_bitmap = new Bitmap(bitmap);
            int[] rgb = new int[3];
            System.Drawing.Color Color_BW;
            System.Drawing.Color Color_Above;
            System.Drawing.Color Color_Below;

            if (radioButton_pixel_black.IsChecked == true)
            {
                Color_Above = System.Drawing.Color.Black;
                Color_Below = System.Drawing.Color.White;
            }
            else
            {
                Color_Above = System.Drawing.Color.White;
                Color_Below = System.Drawing.Color.Black;
            }
           


            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    System.Drawing.Color pixelColor = bitmap.GetPixel(x, y);

                    rgb[0] = pixelColor.R;
                    rgb[1] = pixelColor.G;
                    rgb[2] = pixelColor.B;
                    if (rgb[0]>=Slider_Threshold_R.Value && rgb[1] >= Slider_Threshold_G.Value && rgb[2] >= Slider_Threshold_B.Value)
                    {
                        Color_BW = Color_Above;
                    }
                    else
                    {
                        Color_BW = Color_Below;
                    }

                    tresholded_bitmap.SetPixel(x, y, Color_BW);       
                }


            Image_Orginal.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(tresholded_bitmap.GetHbitmap(),IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));

            Converted_bitmap = tresholded_bitmap;

        }
        private static Boolean isBlackWhite(Bitmap img)
        {
            Boolean result = true;
            for (Int32 h = 0; h < img.Height; h++)
                for (Int32 w = 0; w < img.Width; w++)
                {
                    System.Drawing.Color color = img.GetPixel(w, h);
                    if ((color.R ==0 &&color.G == 0 && color.R == 0) || (color.R == 255 && color.G == 255 && color.R == 255))
                    {
                        
                    }
                    else
                    {
                        result = false;
                        return result;
                    }
                }
            return result;
        }

        #region sliders_Value_changed

        private void Slider_Threshold_R_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textbox_Treshold_R.Text = Slider_Threshold_R.Value.ToString();
        }

        private void Slider_Threshold_G_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textbox_Treshold_G.Text = Slider_Threshold_G.Value.ToString();
        }

        private void Slider_Threshold_B_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textbox_Treshold_B.Text = Slider_Threshold_B.Value.ToString();
        }


        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Bitmap_tresholding();
            Button_enrypt.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (isBlackWhite(Converted_bitmap))
            {
                Random random = new Random();
                int randomNumber;

                int x_2 = 0;
                int y_2 = 0;

                System.Drawing.Color kolorFill = System.Drawing.Color.White;
                System.Drawing.Color kolorFill_2 = System.Drawing.Color.White;

                if (radio_2subpix.IsChecked==true)
                {
                    udzial_1 = new Bitmap(Converted_bitmap.Width * 2, Converted_bitmap.Height );
                    udzial_2 = new Bitmap(Converted_bitmap.Width * 2, Converted_bitmap.Height );

                    for (int y = 0; y < Converted_bitmap.Height - 1; y++)
                        for (int x = 0; x <= Converted_bitmap.Width - 1; x++)
                        {
                            randomNumber = random.Next(0, 2);
                            x_2 =x* 2;

                            if (Converted_bitmap.GetPixel(x, y).R == 0)
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    if (Macierze_2subpiksele.Macierze_czarne.Macierze[randomNumber].udzial1[i] == 1)
                                    {
                                        kolorFill = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        kolorFill = System.Drawing.Color.White;
                                    }

                                    if (Macierze_2subpiksele.Macierze_czarne.Macierze[randomNumber].udzial2[i] == 1)
                                    {
                                        kolorFill_2 = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        kolorFill_2 = System.Drawing.Color.White;
                                    }

                                    switch (i)
                                    {
                                        case 0:
                                            udzial_1.SetPixel(x_2, y, kolorFill);
                                            udzial_2.SetPixel(x_2, y, kolorFill_2);
                                            break;
                                        case 1:
                                            udzial_1.SetPixel(x_2 + 1, y, kolorFill);
                                            udzial_2.SetPixel(x_2 + 1, y, kolorFill_2);

                                            break;
                                    }
                                }
                            }
                            else if (Converted_bitmap.GetPixel(x, y).R == 255)
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    if (Macierze_2subpiksele.Macierze_biale.Macierze[randomNumber].udzial1[i] == 1)
                                    {
                                        kolorFill = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        kolorFill = System.Drawing.Color.White;
                                    }
                                    switch (i)
                                    {
                                        case 0:
                                            udzial_1.SetPixel(x_2, y, kolorFill);
                                            udzial_2.SetPixel(x_2, y, kolorFill);
                                            break;
                                        case 1:
                                            udzial_1.SetPixel(x_2 + 1, y, kolorFill);
                                            udzial_2.SetPixel(x_2 + 1, y, kolorFill);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Coś poszło nie tak wartość jest różna od 0 i 255");
                                return;
                            }
                        }
                }
                else if(radio_4subpix.IsChecked == true)
                {
                    udzial_1 = new Bitmap(Converted_bitmap.Width * 2, Converted_bitmap.Height * 2);
                    udzial_2 = new Bitmap(Converted_bitmap.Width * 2, Converted_bitmap.Height * 2);
                    for (int y = 0; y <= Converted_bitmap.Height - 1; y++)
                        for (int x = 0; x <= Converted_bitmap.Width - 1; x++)
                        {
                            randomNumber = random.Next(0, 6);

                            x_2 = x * 2;
                            y_2 = y * 2;

                            System.Drawing.Color color = Converted_bitmap.GetPixel(x, y);
                            if (color.R == 0)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    if (Macierze_4subpiksele.Macierze_czarne.Macierze[randomNumber].udzial1[i] == 1)
                                    {
                                        kolorFill = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        kolorFill = System.Drawing.Color.White;
                                    }

                                    if (Macierze_4subpiksele.Macierze_czarne.Macierze[randomNumber].udzial2[i] == 1)
                                    {
                                        kolorFill_2 = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        kolorFill_2 = System.Drawing.Color.White;
                                    }

                                    switch (i)
                                    {
                                        case 0:
                                            udzial_1.SetPixel(x_2, y_2, kolorFill);
                                            udzial_2.SetPixel(x_2, y_2, kolorFill_2);
                                            break;
                                        case 1:
                                            udzial_1.SetPixel(x_2 + 1, y_2, kolorFill);
                                            udzial_2.SetPixel(x_2 + 1, y_2, kolorFill_2);

                                            break;
                                        case 2:
                                            udzial_1.SetPixel(x_2, y_2 + 1, kolorFill);
                                            udzial_2.SetPixel(x_2, y_2 + 1, kolorFill_2);
                                            break;
                                        case 3:
                                            udzial_1.SetPixel(x_2 + 1, y_2 + 1, kolorFill);
                                            udzial_2.SetPixel(x_2 + 1, y_2 + 1, kolorFill_2);
                                            break;
                                    }
                                }
                            }
                            else if (color.R == 255)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    if (Macierze_4subpiksele.Macierze_biale.Macierze[randomNumber].udzial1[i] == 1)
                                    {
                                        kolorFill = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        kolorFill = System.Drawing.Color.White;
                                    }
                                    switch (i)
                                    {
                                        case 0:
                                            udzial_1.SetPixel(x_2, y_2, kolorFill);
                                            udzial_2.SetPixel(x_2, y_2, kolorFill);
                                            break;
                                        case 1:
                                            udzial_1.SetPixel(x_2 + 1, y_2, kolorFill);
                                            udzial_2.SetPixel(x_2 + 1, y_2, kolorFill);
                                            break;
                                        case 2:
                                            udzial_1.SetPixel(x_2, y_2 + 1, kolorFill);
                                            udzial_2.SetPixel(x_2, y_2 + 1, kolorFill);
                                            break;
                                        case 3:
                                            udzial_1.SetPixel(x_2 + 1, y_2 + 1, kolorFill);
                                            udzial_2.SetPixel(x_2 + 1, y_2 + 1, kolorFill);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Coś poszło nie tak wartość jest różna od 0 i 255");
                                return;
                            }
                        }
                }
                else
                {
                    MessageBox.Show("cos nie tak z radio buttonami");
                }

                Image_part1.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(udzial_1.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(udzial_1.Width, udzial_1.Height));

                Image_part2.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(udzial_2.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(udzial_2.Width, udzial_2.Height));

                Image_part2.Opacity = 0;

                Button_SaveUdzialy.IsEnabled = true;

            }
            else
            {
                MessageBox.Show("Obraz nie jest czarno biały. \n Proszę użyć progowania");
                return;
            }
        }

        private void Button_SaveUdzialy_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save As...";
            saveFileDialog.Filter = "bmp (*.bmp)|*.bmp";
            saveFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\Kryptografia Wizualna";


            if (saveFileDialog.ShowDialog() == true)
            {
                udzial_1.Save(saveFileDialog.FileName);
            }

            saveFileDialog.Reset();
            saveFileDialog.Title = "Save As...";
            saveFileDialog.Filter = "bmp (*.bmp)|*.bmp";
            saveFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\Kryptografia Wizualna";

            if (saveFileDialog.ShowDialog() == true)
            {
                udzial_2.Save(saveFileDialog.FileName);
            }
        }

        private void Slider_transparency_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Image_part2.Opacity = Slider_transparency.Value / 255;
        }

        private void Button_Load_Parts_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\Kryptografia Wizualna";
            openFileDialog.Filter = "All files(*.*) |*.*|BMP (*.bmp)|*.bmp| PNG (*.png)|*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                Loaded_bitmap_1 = new Bitmap(path);
                Image_part1_2.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(Loaded_bitmap_1.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
               BitmapSizeOptions.FromWidthAndHeight(Loaded_bitmap_1.Width, Loaded_bitmap_1.Height));

                
                openFileDialog.FileName = "";

                if (openFileDialog.ShowDialog() == true)
                {
                    path = openFileDialog.FileName;
                    Loaded_bitmap_2 = new Bitmap(path);
                    Image_part2_2.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(Loaded_bitmap_2.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
               BitmapSizeOptions.FromWidthAndHeight(Loaded_bitmap_2.Width, Loaded_bitmap_2.Height));
                    

                    if (!isBlackWhite(Loaded_bitmap_1) ||  !isBlackWhite(Loaded_bitmap_2))
                    {
                        MessageBox.Show("Obrazy nie są czarno białe");
                        return;
                    }
                    else if(!(Loaded_bitmap_1.Height == Loaded_bitmap_2.Height) || !(Loaded_bitmap_1.Width == Loaded_bitmap_2.Width))
                    {
                        MessageBox.Show("Obrazy niemają takiego samego rozmiaru");
                        return;
                    }
                    else
                    {
                        Button_Decrypt.IsEnabled = true;
                    }

                }
            }
        }

        private void Button_Decrypt_Click(object sender, RoutedEventArgs e)
        {
            int x_2 = 0;
            int y_2 = 0;

            if (radio_2subpix_2.IsChecked == true)
            {
                result = new Bitmap((Loaded_bitmap_1.Width / 2), Loaded_bitmap_1.Height );

                for (int y = 0; y < result.Height - 1; y++)
                    for (int x = 0; x < result.Width - 1; x++)
                    {
                        x_2 = x * 2;

                        if (Loaded_bitmap_1.GetPixel(x_2, y).Equals(Loaded_bitmap_2.GetPixel(x_2, y))  && Loaded_bitmap_1.GetPixel(x_2 + 1, y).Equals(Loaded_bitmap_2.GetPixel(x_2 + 1, y)))
                        {
                            result.SetPixel(x, y, System.Drawing.Color.White);
                        }
                        else
                        {
                            result.SetPixel(x, y, System.Drawing.Color.Black);
                        }
                    }
            }
            else if (radio_4subpix_2.IsChecked == true)
            {
                result = new Bitmap(Loaded_bitmap_1.Width/2, Loaded_bitmap_1.Height / 2);

                for (int y = 0; y < result.Height - 2; y++)
                    for (int x = 0; x < result.Width - 2; x++)
                    {
                        x_2 = x * 2;
                        y_2 = y * 2;

                        if (Loaded_bitmap_1.GetPixel(x_2, y_2).R == Loaded_bitmap_2.GetPixel(x_2, y_2).R
                            && Loaded_bitmap_1.GetPixel(x_2 + 1, y_2).R == Loaded_bitmap_2.GetPixel(x_2 + 1, y_2).R
                            && Loaded_bitmap_1.GetPixel(x_2 , y_2 + 1).R == Loaded_bitmap_2.GetPixel(x_2, y_2 + 1).R
                            && Loaded_bitmap_1.GetPixel(x_2 + 1, y_2 + 1).R == Loaded_bitmap_2.GetPixel(x_2 + 1, y_2 + 1).R
                            )
                        {
                            result.SetPixel(x, y, System.Drawing.Color.White);
                        }
                        else
                        {
                            result.SetPixel(x, y, System.Drawing.Color.Black);
                        }
                    }
            }
            else
            {

                MessageBox.Show("Cos poszlo nie tak");
                return;

            }

                Image_decrypted.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(result.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(result.Width, result.Height));

                Button_SaveDecrypted.IsEnabled = true;
        }

        private void Button_SaveDecrypted_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save As...";
            saveFileDialog.Filter = "bmp (*.bmp)|*.bmp";
            saveFileDialog.InitialDirectory = "C:\\Users\\Hubi\\Desktop\\Ten semestr\\Pody\\Kryptografia Wizualna";


            if (saveFileDialog.ShowDialog() == true)
            {
                result.Save(saveFileDialog.FileName);
            }

        }
    }
}
