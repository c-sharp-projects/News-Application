using NewsAPI.Models;
using NewsAPI.Constants;
using NewsAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace News
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string ApiKey = "Api key"; // generate your own api key
        public NewsApiClient news;
        public MainWindow()
        {
            InitializeComponent();
            news = new NewsApiClient(ApiKey);
            GetNews("General");
            LoadIcon();
        }
        private void LoadIcon()
        {
            Back.Content = new Image { Source = new BitmapImage(new Uri(Resource.backbtn)) };
        }
        private void ActionListener(object sender, RoutedEventArgs e)
        {
            grid2.Visibility = Visibility.Hidden;
            Newsgrid.Visibility = Visibility.Visible;
            Button btn = (Button)sender;            
            string Category = btn.Uid.ToString();
            GetNews(Category);
        }
        public async void GetNews(string Category)
        {
            ArticlesResult ArticleRes = null;

            try
            {
                await Task.Run(() =>
                {
                    switch (Category)
                    {
                        case "Business":
                            ArticleRes = news.GetTopHeadlines(new TopHeadlinesRequest
                            {
                                Country = Countries.IN,
                                Language = Languages.EN,
                                Category = Categories.Business
                            });

                            break;

                        case "Entertainment":
                            ArticleRes = news.GetTopHeadlines(new TopHeadlinesRequest
                            {
                                Country = Countries.IN,
                                Language = Languages.EN,
                                Category = Categories.Entertainment
                            });
                            break;

                        case "General":
                            ArticleRes = news.GetTopHeadlines(new TopHeadlinesRequest
                            {
                                Country = Countries.IN,
                                Language = Languages.EN,

                            });
                            break;

                        case "Health":
                            ArticleRes = news.GetTopHeadlines(new TopHeadlinesRequest
                            {
                                Country = Countries.IN,
                                Language = Languages.EN,
                                Category = Categories.Health
                            });
                            break;

                        case "Science":
                            ArticleRes = news.GetTopHeadlines(new TopHeadlinesRequest
                            {
                                Country = Countries.IN,
                                Language = Languages.EN,
                                Category = Categories.Science
                            });
                            break;

                        case "Sports":
                            ArticleRes = news.GetTopHeadlines(new TopHeadlinesRequest
                            {
                                Country = Countries.IN,
                                Language = Languages.EN,
                                Category = Categories.Sports
                            });
                            break;

                        case "Technology":
                            ArticleRes = news.GetTopHeadlines(new TopHeadlinesRequest
                            {
                                Country = Countries.IN,
                                Language = Languages.EN,
                                Category = Categories.Technology
                            });
                            break;

                    }


                    if (ArticleRes.Status != NewsAPI.Constants.Statuses.Ok)
                    {
                        MessageBox.Show("Error");
                        return;
                    }

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        UpdateNews(ArticleRes);
                    }));
                });


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public BitmapImage LoadImage(string url)
        {
            BitmapImage bi = null;
            try
            {
                if (url == null)
                {
                    url = AppDomain.CurrentDomain.BaseDirectory + @"News\Default\NoPreview.jpg";
                }

                bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(url);
                bi.EndInit();
            }
            catch (Exception e)
            {
                url = AppDomain.CurrentDomain.BaseDirectory + @"News\Default\NoPreview.jpg";
                bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(url);
                bi.EndInit();
            }
            return bi;
        }
        public void UpdateNews(ArticlesResult articlesResult)
        {
            NewsInfo[] infos = new NewsInfo[articlesResult.Articles.Count];

            int i = 0;
            foreach (var articles in articlesResult.Articles)
            {
                infos[i] = new NewsInfo
                {
                    Author = articles.Author,
                    Description = articles.Description,
                    PublishedAt = articles.PublishedAt.ToString(),
                    Title = articles.Title,
                    Url = articles.Url,
                    UrlToImage = articles.UrlToImage,
                    ImageData = LoadImage(articles.UrlToImage)
                };
                i++;
            }

            Newslb.ItemsSource = infos.ToList();
        }
        private void ListBoxListener(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            NewsInfo newsInfo = (NewsInfo)lb.SelectedItem;

            if(newsInfo == null)
            {
                return;
            }
            Titletb.Text = newsInfo.Title;
            NewsImg.Source = newsInfo.ImageData;
            Authortb.Text = newsInfo.Author;
            Publishedtb.Text = newsInfo.PublishedAt;
            Descriptiontb.Text = newsInfo.Description;
            hlink.NavigateUri = new Uri(newsInfo.Url);

            Newsgrid.Visibility = Visibility.Hidden;
            grid2.Visibility = Visibility.Visible;

            lb.SelectedItem = null;   // For double selection of a single item
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            grid2.Visibility = Visibility.Hidden;
            Newsgrid.Visibility = Visibility.Visible;
        }
        private void Hlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }   
   
}
