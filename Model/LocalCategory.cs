using System;
using System.Collections.Generic;
using System.Linq;

namespace BoshokuDemo1.Model
{
    public class LocalCategory
    {
        public string val
        {
            get;
            set;
        }

        public string icon
        {
            get;
            set;
        }

        public List<LocalCategory> sub
        {
            get;
            set;
        }

        public bool isSub
        {
            get;
            set;
        }

        public string key
        {
            get;
            set;
        }
        public string rightIcon
        {
            get;
            set;
        }
        public string newNotify
        {
            get;
            set;
        }

        public static List<LocalCategory> getListMain()
        {

            var category = new LocalCategory()
            {

                val = "Ayın Ödüllü Çalışanları",
                isSub = true,
                key = "-L-wuEMDkegr1fQhv7PP",
                icon = "monthuser",
                sub = new List<LocalCategory>()
                {

                }
                ,
                rightIcon = "right"
            };
            var months_ = Enum.GetNames(typeof(Months));
            for (int i = 0; i < 12; i++)
            {
                category.sub.Add(new LocalCategory()
                {
                    val = months_[i],
                    isSub = false,
                    icon = "subicon",
                    rightIcon = "right"
                });

            }

            var categoryHelp = new LocalCategory
            {
                val = "Sosyal Sorumluluk",
                icon = "socialrespon",
                key = "-L-wuHAlGsKeH9Ud6zsf",
                isSub = false,
                rightIcon = "right"
            };
            var categoryBusiness = new LocalCategory
            {
                val = "Şirket İçi Duyurular",
                icon = "busnotify",
                key = "-L00tlD3V8q_dUQ5ptk0",
                rightIcon = "right",
                isSub = false
            };
            var categoryWorkError = new LocalCategory
            {
                val = "İş Kazaları Bilgilendirmeleri",
                icon = "accwork",
                key = "-L-wuJFc6IDukUVy99c-",
                rightIcon = "right",
                isSub = false

            };
            var categoryProductionplan = new LocalCategory
            {
                val = "Çalışma Planı",
                icon = "productplan",
                key = "-L-wuLFqzXLpgWQh-Xzr",
                rightIcon = "right",
                isSub = false
            };

            var categoryFamilyAdd = new LocalCategory
            {
                val = "Ailemize Katılanlar",
                icon = "familyparti",
                key = "-L-wuMuTp9QZbiuNEu7Z",
                rightIcon = "right",
                isSub = false
            };
            var categoryServiceHouse = new LocalCategory
            {
                val = "Servis Saatleri",
                icon = "servicehours",
                key = "-L-wuQ8iOb-kJg97FZz4",
                rightIcon = "right",
                isSub = false
            };
            var categoryVisitors = new LocalCategory
            {
                key = "-L-wuSbUZwOoeoIr0dOk",
                val = "Ziyaretçilerimiz",
                icon = "visitor",
                rightIcon = "right",
                isSub = false
            };

            var categoryPositions = new LocalCategory
            {
                key = "-L-wuOWSBbJ0L7TQbhID",
                val = "Açık Pozisyonlar",
                icon = "openpositions",
                rightIcon = "right",
                isSub = false
            };


            var categoryFoodMenu = new LocalCategory
            {
                key = "-L-wuUBNyedjQzCJtMqY",
                val = "Yemek Menüsü",
                icon = "eatmenu",
                rightIcon = "right",
                isSub = false
            };


            var seasonsData = Enum.GetNames(typeof(seasons)).ToList();
            var iconList = new string[] { "ic_winter", "ic_spring", "ic_summer", "ic_autumn" };
            var categorySeasons = new List<LocalCategory>();
            for (int i = 0; i < seasonsData.Count; i++)
            {
                categorySeasons.Add(new LocalCategory()
                {
                    val = seasonsData[i],
                    icon = iconList[i],
                    rightIcon = "right",
                    isSub = false
                });
            }


            //       var categryERActivity = new LocalCategory
            //       {

            //           val = "ER Etkinlikleri",
            //           isSub = true,
            //           icon = "eractivity",
            //           rightIcon = "right",
            //           key="-eR1234",
            //           sub = new List<LocalCategory>() {
            // new LocalCategory() {
            //                   val = "Gezi Klübü", isSub = true,key="-L90UzTNtXkCdYCSIFDI", sub = categorySeasons, icon = "ic_store",rightIcon ="right"
            //  },
            //  new LocalCategory() {
            //                   val = "Doğa Sporları", isSub = true,key="-L90VCK67f70U5rN_8LC", sub = categorySeasons, icon = "ic_rowing",rightIcon ="right"
            //  },
            //  new LocalCategory() {
            //                   val = "Aile Günü", isSub = true,key="-L90VFb99ytyj_sgqQPN", icon = "ic_loyalty",rightIcon ="right",sub = categorySeasons
            //  },
            //}
            //};

            var categoryActivityFature = new LocalCategory
            {

                val = "Gerçekleşecek Etkinlikler",
                isSub = false,
                icon = "ic_fature",
                rightIcon = "right",
                key = "-AC123"

            };
            var categoryActivityPast = new LocalCategory
            {

                val = "Gerçekleşen Etkinlikler",
                isSub = false,
                icon = "ic_past",
                rightIcon = "right",
                key = "-AC124"

            };



            var hearList = new List<LocalCategory>() {
            category,
            categoryHelp,
            categoryBusiness,
            categoryWorkError,
            categoryProductionplan,
             categoryFamilyAdd,
            categoryPositions,
            categoryServiceHouse,
            categoryVisitors,
            categoryFoodMenu
           };
            var activityList = new List<LocalCategory>() {
                categoryActivityFature,
                categoryActivityPast
            };

            #region TBT hadkkında 

            var categoryHWAbout = new LocalCategory
            {
                key = "-123H1",
                val = "HWA Hakkında",
                icon = "ic_info",
                rightIcon = "right",
                isSub = false
            };
            var categoryHWAProfile = new LocalCategory
            {
                key = "-123H2",
                val = "Youtube",
                icon = "ic_home",
                rightIcon = "right",
                isSub = false
            };
            var categoryFacebook = new LocalCategory
            {
                key = "-123H3",
                val = "Facebook",
                icon = "ic_about",
                rightIcon = "right",
                isSub = false
            };
            var categoryInstagram = new LocalCategory
            {
                key = "-123H4",
                val = "Instagram",
                icon = "ic_mail",
                rightIcon = "right",
                isSub = false
            };
            var categoryTwitter = new LocalCategory
            {
                key = "-123H5",
                val = "Twitter",
                icon = "ic_trophy",
                rightIcon = "right",
                isSub = false
            };
            var categoryKodCocuk = new LocalCategory
            {
                key = "-123H6",
                val = "Kod Çocuk",
                icon = "ic_actors",
                rightIcon = "right",
                isSub = false
            };
            var categoryMedium = new LocalCategory
            {
                key = "-123H7",
                val = "Medium",
                icon = "ic_message",
                rightIcon = "right",
                isSub = false
            };

            #endregion


            var categoryHear = new LocalCategory()
            {
                val = "Duydunuz mu?",
                isSub = true,
                key = "-D",
                rightIcon = "right",
                sub = hearList,
                icon = "ic_megaphone"

            };
            var categoryClup = new LocalCategory()
            {
                val = "HWA Club",
                isSub = true,
                key = "-D",
                rightIcon = "right",
                sub = activityList,
                icon = "ic_outdoors"


            };
            var categorySurvey = new LocalCategory()
            {
                val = "Anket",
                isSub = false,
                key = "-123YA",
                rightIcon = "right",
                icon = "ic_survey_event"


            };
            var categoryHome = new LocalCategory()
            {
                val = "Bizim Köy",
                isSub = true,
                rightIcon = "right",
                key = "-123H",
                icon = "ic_village",
                sub = new List<LocalCategory>() {
             categoryHWAbout,categoryHWAProfile,categoryFacebook,categoryInstagram,categoryTwitter,categoryKodCocuk,categoryMedium}

            };
            var categoryEvent = new LocalCategory()
            {
                val = "Yarışma",
                isSub = false,
                key = "-123YE",
                rightIcon = "right",
                icon = "ic_drawer_event"
            };



            return new List<LocalCategory>(){
                categoryHear,
                categoryClup,
                categorySurvey,
                categoryEvent,
                categoryHome
            };

        }

    }

    enum Months
    {

        Ocak = 1,
        Şubat = 2,
        Mart = 3,
        Nisan = 4,
        Mayıs = 5,
        Haziran = 6,
        Temmuz = 7,
        Ağustos = 8,
        Eylül = 9,
        Ekim = 10,
        Kasım = 11,
        Aralık = 12
    }
    enum seasons
    {
        BeyazKış = 21,
        MevsimBahar = 22,
        SıcakYaz = 23,
        Sonbahar = 24
    }
}