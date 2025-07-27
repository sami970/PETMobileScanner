namespace PETScanner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class SettingsService
    {
        public static string MobileId
        {
            get => Preferences.Get(nameof(MobileId), string.Empty);
            set => Preferences.Set(nameof(MobileId), value);
        }

        public static string Password
        {
            get => Preferences.Get(nameof(Password), string.Empty);
            set => Preferences.Set(nameof(Password), value);

        }

            public static string OnValidPassUrl
        {
            get => Preferences.Get(nameof(OnValidPassUrl), string.Empty);
            set => Preferences.Set(nameof(OnValidPassUrl), value);
        }

        public static string OnhandDataURL
        {
            get => Preferences.Get(nameof(OnhandDataURL), string.Empty);
            set => Preferences.Set(nameof(OnhandDataURL), value);
        }

        public static string CreateJournalURL
        {
            get => Preferences.Get(nameof(CreateJournalURL), string.Empty);
            set => Preferences.Set(nameof(CreateJournalURL), value);
        }

        public static string CreateJournalLineURL
        {
            get => Preferences.Get(nameof(CreateJournalLineURL), string.Empty);
            set => Preferences.Set(nameof(CreateJournalLineURL), value);
        }

        public static string PostNewJournalURL
        {
            get => Preferences.Get(nameof(PostNewJournalURL), string.Empty);
            set => Preferences.Set(nameof(PostNewJournalURL), value);
        }


        public static string PostNewAdjustmentJournalURL
        {
            get => Preferences.Get(nameof(PostNewAdjustmentJournalURL), string.Empty);
            set => Preferences.Set(nameof(PostNewAdjustmentJournalURL), value);
        }


        public static string DefaultLocation
        {
            get => Preferences.Get(nameof(DefaultLocation), string.Empty);
            set => Preferences.Set(nameof(DefaultLocation), value);
        }

        public static string TransferJournalType
        {
            get => Preferences.Get(nameof(TransferJournalType), string.Empty);
            set => Preferences.Set(nameof(TransferJournalType), value);
        }

        public static string AdjustmentJournalType
        {
            get => Preferences.Get(nameof(AdjustmentJournalType), string.Empty);
            set => Preferences.Set(nameof(AdjustmentJournalType), value);
        }
        public static string DefaultUrl
        {
            get => Preferences.Get(nameof(DefaultUrl), string.Empty);
            set => Preferences.Set(nameof(DefaultUrl), value);
        }
        

        public static string DefaultPort
        {
            get => Preferences.Get(nameof(DefaultPort), string.Empty);
            set => Preferences.Set(nameof(DefaultPort), value);
        }
    }
}
