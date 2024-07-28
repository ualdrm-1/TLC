using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfAnimatedGif;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace TLC
{   
    public abstract class Object
    {
        public int health { get; set; }
        public Object() { }
        public Object(int health)
        {
            this.health = health;
        }
        public void amimoving()
        {
            if (health > 0) { Console.WriteLine("1"); } else { Console.WriteLine("0"); }
        }
    }
    public class Player : Object
    {
        public int damage { get; set; }
        public int speed { get; set; }
        public int force { get; set; }
        public bool goLeft { get; set; }
        public bool goRight { get; set; }
        public bool attack { get; set; }
        public int score { get; set; }
        public Player() { }
        public Player(int health, int damage, int speed, int force,int score, bool goLeft, bool goRight, bool attack):base(health)
        {
            this.damage = damage;
            this.speed = speed;
            this.force = force;
            this.score = score;
            this.goLeft = goLeft;
            this.goRight = goRight;
            this.attack = attack;
        }
    }
    class Enemy : Object
    {
        public int damage { get; set; }
        public int speed { get; set; }
        public bool attack { get; set; }
        public int limit { get; set; }
        public int enemycounter { get; set; }
        public Enemy() { }
        public Enemy(int health, int damage, int speed, bool attack, int limit, int enemycounter) : base(health)
        {
            this.damage = damage;
            this.speed = speed;
            this.attack = attack;
            this.limit = limit;
            this.enemycounter = enemycounter;
        }
    }
    public class AnyAnimation
    {
        public int total;
        public int spritew;
        public int spriteh;
        public int currentsprite;

        public AnyAnimation(int total, int spritew, int spriteh, int currentsprite)
        {
            this.total = total;
            this.spritew = spritew;
            this.spriteh = spriteh;
            this.currentsprite = currentsprite;
        }
        public void SpriteAnimation(Image image, string str, int totalsize, Player pl, bool isitplayer)
        {
            BitmapImage spriteSheet = new BitmapImage(new Uri(str, UriKind.RelativeOrAbsolute));
            Int32Rect sourceRect = new Int32Rect(currentsprite * spritew, 0, spritew, spriteh);
            CroppedBitmap cropped = new CroppedBitmap(spriteSheet, sourceRect);
            if (isitplayer)
            {
                if (pl.attack)
                {
                    image.Source = cropped;
                    currentsprite = (currentsprite + 1) % (total - totalsize);
                }
                else
                {
                    image.Source = cropped;
                    currentsprite = (currentsprite + 1) % (total - totalsize);
                }
            }
            else
            {
                image.Source = cropped;
                currentsprite = (currentsprite + 1) % (total - totalsize);
            }
        }
    }
    public partial class Game : Window
    {
        public MediaPlayer _mpBgr = new MediaPlayer();
        static System.Timers.Timer stamina = new System.Timers.Timer(2000);
        DispatcherTimer dp;
        public int difficulty { get; set; }
        public int attackcount;
        public int TotalScore { get; set; }
        public bool gamestop;
        public int bonuscount;
        public SoundPlayer _spSelect = new SoundPlayer(@"C:\Users\Huawei\Downloads\svist-razmahivayuschego-mecha.wav");
        DispatcherTimer gameTimer = new DispatcherTimer();

        Random rand = new Random();

        Rect playerhitbox;
        Rect groundhitbox;
        Rect towerhitbox;

        Player MainPlayer = new Player(100, 50, 7, 10,0, false, false, false);
        Enemy enemy1 = new Enemy(100, 2, 4, false, 50,50);

        AnyAnimation TowerFlag = new AnyAnimation(5, 60, 58, 0);
        AnyAnimation MainHero = new AnyAnimation(10, 100, 64, 0);

        int spritecounter; bool gameover;
        private int i = 0, win = 10;
        private int dif;

        public Game(int dif)
        {
            difficulty = dif;
            switch (difficulty)
            {
                case 1:
                    enemy1.damage -= 1;
                    enemy1.speed -= 1;
                    enemy1.limit += 20;
                    enemy1.enemycounter += 20;
                    win = 4;
                    break;
                case 3:
                    enemy1.damage += 8;
                    enemy1.speed += 8;
                    enemy1.limit -= 40;
                    enemy1.enemycounter -= 40;
                    win = 100;
                    break;
            }
            InitializeComponent();
            stamina.Elapsed += TimerElapsed;
            stamina.AutoReset = false;
            MyCanvas.Focus();
            gameTimer.Tick += GameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(50);
            dp = new DispatcherTimer();
            dp.Interval = TimeSpan.FromSeconds(1);
            dp.Tick += timetick;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            MainPlayer.attack = false;
            stamina.Stop();
        }

        private void timetick(object sender, EventArgs e)
        {   
            if (gameover == false)
            {
                i++;
                TimerLabel.Content = i.ToString();
            }
            if (gamestop)
            {
                dp.Stop();
            }
            else
            {
                dp.Start();
            }
            
        }
        public void EnemyMake()
        {
            Image newEnemy1 = new Image()
            {
                Tag = "enemy1",
            };
            spritecounter = rand.Next(1, 6);
            BitmapImage imageSource = new BitmapImage();
            switch (spritecounter)
            {
                case 1:
                    imageSource.BeginInit();
                    imageSource.UriSource = new Uri(@"C:\Users\Huawei\Downloads\demon-idle.gif");
                    imageSource.EndInit();

                    newEnemy1.Width = 180;
                    newEnemy1.Height = 210;

                    ImageBehavior.SetAnimatedSource(newEnemy1, imageSource);
                    Canvas.SetTop(newEnemy1, 290);
                    break;
                case 2:
                    imageSource.BeginInit();
                    imageSource.UriSource = new Uri(@"C:\Users\Huawei\Downloads\ghost-idle.gif");
                    imageSource.EndInit();

                    newEnemy1.Width = 100;
                    newEnemy1.Height = 100;

                    ImageBehavior.SetAnimatedSource(newEnemy1, imageSource);
                    Canvas.SetTop(newEnemy1, 345);
                    break;
                case 3:
                    imageSource.BeginInit();
                    imageSource.UriSource = new Uri(@"C:\Users\Huawei\Downloads\nightmare-galloping.gif");
                    imageSource.EndInit();

                    newEnemy1.Width = 160;
                    newEnemy1.Height = 180;

                    ImageBehavior.SetAnimatedSource(newEnemy1, imageSource);
                    Canvas.SetTop(newEnemy1, 330);
                    break;
                case 4:
                    imageSource.BeginInit();
                    imageSource.UriSource = new Uri(@"C:\Users\Huawei\Downloads\gothicvania patreon collection\gothicvania patreon collection\Fire-Skull-Files\GIF\fire-skull.gif");
                    imageSource.EndInit();

                    newEnemy1.Width = 90;
                    newEnemy1.Height = 125;

                    ImageBehavior.SetAnimatedSource(newEnemy1, imageSource);
                    Canvas.SetTop(newEnemy1, 335);
                    break;
            }
            Canvas.SetLeft(newEnemy1, rand.Next(1920, 3000));
            
            MyCanvas.Children.Add(newEnemy1);
            GC.Collect();
        }
        //public void makebonus()
        //{
        //    Image Bonus = new Image()
        //    {
        //        Tag = "bonus"
        //    };
        //    BitmapImage BonusSource = new BitmapImage();
        //    BonusSource.BeginInit();
        //    BonusSource.UriSource = new Uri(@"D:\studies\TLC\TLC\images\pixel-heart.gif");
        //    BonusSource.EndInit();
        //    ImageBehavior.SetAnimatedSource(Bonus, BonusSource);
        //    Bonus.Width = 40;
        //    Bonus.Height = 40;

        //    Canvas.SetTop(Bonus, 425);
        //    Canvas.SetLeft(Bonus, rand.Next(500, 1000));
        //    MyCanvas.Children.Add(Bonus);
        //}
        public void GameTimerEvent(object sender, EventArgs e)
        {
            if (gameover == false && gamestop==false)
            {
                Score.Content = "Score: " + MainPlayer.score;
                TotalScore = MainPlayer.score+1;

                if (MainPlayer.goLeft == true && Canvas.GetLeft(player) > 5)
                {
                    Canvas.SetLeft(player, Canvas.GetLeft(player) - MainPlayer.speed);
                    Canvas.SetLeft(HealthBar_Copy, Canvas.GetLeft(HealthBar_Copy) - MainPlayer.speed);
                }
                if (MainPlayer.goRight == true && Canvas.GetLeft(player) + (player.Width + 20) < System.Windows.Application.Current.MainWindow.Width + 600)
                {
                    Canvas.SetLeft(player, Canvas.GetLeft(player) + MainPlayer.speed);
                    Canvas.SetLeft(HealthBar_Copy, Canvas.GetLeft(HealthBar_Copy) + MainPlayer.speed);
                }

                enemy1.enemycounter--;

                playerhitbox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
                groundhitbox = new Rect(Canvas.GetLeft(ground), Canvas.GetTop(ground), ground.Width, ground.Height);
                towerhitbox = new Rect(Canvas.GetLeft(tower), Canvas.GetTop(tower), tower.Width - 100, tower.Height);

                if (playerhitbox.IntersectsWith(groundhitbox))
                {
                    Canvas.SetTop(player, Canvas.GetTop(ground) + player.Height);
                    Console.WriteLine("yes");
                }
                if (playerhitbox.IntersectsWith(towerhitbox))
                {
                    Canvas.SetLeft(player, Canvas.GetLeft(player) + MainPlayer.speed);
                    Canvas.SetLeft(HealthBar_Copy, Canvas.GetLeft(HealthBar_Copy) + MainPlayer.speed);
                    Console.WriteLine("aasadsada");
                }

                if (enemy1.enemycounter < 0)
                {
                    EnemyMake();
                    enemy1.enemycounter = enemy1.limit;
                }

                foreach (var x in MyCanvas.Children.OfType<Image>())
                {
                    Rect enemyhitbox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (x is Image && (string)x.Tag == "enemy1")
                    {
                        if (enemyhitbox.IntersectsWith(towerhitbox))
                        {
                            HealthBar.Value -= enemy1.damage;
                        }

                        if (playerhitbox.IntersectsWith(enemyhitbox) && MainPlayer.attack == true)
                        {
                            MainPlayer.health += 5;
                            HealthBar_Copy.Value+= 5;
                            Canvas.SetLeft(x, -1000);
                            MainPlayer.score++;
                        }

                        if (playerhitbox.IntersectsWith(enemyhitbox) && MainPlayer.attack == false)
                        {
                            MainPlayer.health -= enemy1.damage;
                            HealthBar_Copy.Value -= enemy1.damage;
                        }

                        if (towerhitbox.IntersectsWith(enemyhitbox))
                        {
                            Canvas.SetLeft(x, Canvas.GetLeft(x) - 0);
                        }
                        else
                        {
                            Canvas.SetLeft(x, Canvas.GetLeft(x) - enemy1.speed);
                        }

                        if (MainPlayer.score == win)
                        {
                            WINGAME.Visibility = Visibility.Visible;
                            EXIT_GAME.Visibility= Visibility.Visible;
                            gameover = true;
                            _mpBgr.Stop();
                            Close_Game.IsEnabled = false;

                        }
                        if (HealthBar.Value == 0 || MainPlayer.health == 0)
                        {
                            LOSEGAME.Visibility=Visibility.Visible;
                            EXIT_GAME.Visibility = Visibility.Visible;
                            gameover = true;
                            _mpBgr.Stop();
                            Close_Game.IsEnabled = false;
                            Close_Game.Visibility = Visibility.Hidden;
                        }
                    }
                    foreach(var y in MyCanvas.Children.OfType<Image>())
                    {
                        Rect bonushitbox = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                        if (x is Image && (string)x.Tag == "bonus")
                        {
                            if (playerhitbox.IntersectsWith(bonushitbox))
                            {
                                Canvas.SetLeft(y, -1000);
                                MainPlayer.health += 10;
                                HealthBar_Copy.Value += 10;
                            }
                        }
                    }
                }
                TowerFlag.SpriteAnimation(flag, "C:\\Users\\Huawei\\Downloads\\flag animation.png", 6, MainPlayer, false);
                if (MainPlayer.attack != true && MainPlayer.goLeft != true && MainPlayer.goRight != true)
                {
                    MainHero.SpriteAnimation(player, "C:\\Users\\Huawei\\Downloads\\Knight_player_1.3\\Knight_player\\Idle_KG_2.png",6,MainPlayer, true);
                }
                if (MainPlayer.attack)
                {
                    MainHero.SpriteAnimation(player, "C:\\Users\\Huawei\\Downloads\\Knight_player_1.3\\Knight_player\\Attack_KG_2.png", 4, MainPlayer, true);
                    //attackcount=rand.Next(1, 4);
                    //switch (attackcount) {
                    //    case 1:
                    //MainHero.SpriteAnimation(player, "C:\\Users\\Huawei\\Downloads\\Knight_player_1.3\\Knight_player\\Attack_KG_2.png", 4, MainPlayer, true);
                    //        break;
                    //    case 2:
                    //MainHero.SpriteAnimation(player, "C:\\Users\\Huawei\\Downloads\\Knight_player_1.3\\Knight_player\\Attack_KG_1.png", 4, MainPlayer, true);
                    //        break;
                    //    case 3:
                    //        MainHero.SpriteAnimation(player, "C:\\Users\\Huawei\\Downloads\\Knight_player_1.3\\Knight_player\\Attack_KG_3.png", 4, MainPlayer, true);
                    //        break;
                    //    case 4:
                    //        MainHero.SpriteAnimation(player, "C:\\Users\\Huawei\\Downloads\\Knight_player_1.3\\Knight_player\\Attack_KG_4.png", 4, MainPlayer, true);
                    //        break;
                    //}
                }
                if (MainPlayer.goRight)
                {
                    MainHero.SpriteAnimation(player, "C:\\Users\\Huawei\\Downloads\\Knight_player_1.3\\Knight_player\\Walking_KG_2.png", 3, MainPlayer, true);
                }
                if (MainPlayer.goLeft)
                {
                    MainHero.SpriteAnimation(player, "C:\\Users\\Huawei\\Downloads\\Knight_player_1.3\\Knight_player\\Walking_KG_3.png", 3, MainPlayer, true);
                }
            }
        }
        public void StartGame()
        {
            gameTimer.Start();
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                MainPlayer.goLeft = true;
                MainPlayer.attack = false;
                stamina.Stop();
            }
            if (e.Key == Key.D)
            {
                MainPlayer.attack = false;
                MainPlayer.goRight = true;
                stamina.Stop();
            }
            if (e.Key == Key.Space)
            {
                if (!MainPlayer.goLeft && !MainPlayer.goRight)
                {
                    MainPlayer.attack = true;
                    MainHero.currentsprite = 0;
                    _spSelect.Play();
                    if (!stamina.Enabled)
                    {
                        stamina.Start();
                    }
                }
            }
            if (gameover == true)
            {
                if (e.Key == Key.Escape)
                {
                    this.Close();
                }
            }
            if (e.Key == Key.Enter)
            {
                PressEnter.Visibility = Visibility.Hidden;
                dp.Start();
                StartGame();
                _mpBgr.Open(new Uri(@"C:\Users\Huawei\Downloads\PhaseShift(chosic.com).mp3"));
                _mpBgr.Play();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                MainPlayer.goLeft = false;
                MainPlayer.attack = false;
                MainHero.currentsprite = 0;
            }
            if (e.Key == Key.D)
            {
                MainPlayer.goRight = false;
                MainPlayer.attack = false;
                MainHero.currentsprite = 0;
            }
        }
        private void Pause_Game_Checked(object sender, RoutedEventArgs e)
        {
            gamestop = true;
            PauseGame.Visibility=Visibility.Visible;
            _mpBgr.Pause();
        }

        private void isUnchecked(object sender, RoutedEventArgs e)
        {
            gamestop = false;
            PauseGame.Visibility = Visibility.Collapsed;
            dp.Start();
            _mpBgr.Play();
        }

        private void Close_Game_Click(object sender, RoutedEventArgs e)
        {
            _mpBgr.Close();
            this.Close();
        }
    }
}
