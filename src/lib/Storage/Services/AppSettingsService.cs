﻿using System;
using System.Collections.Generic;
using System.IO;
using LiteDB;
using Storage.Enums;
using Storage.Interfaces;
using Storage.Models;

namespace Storage.Services
{
    public class AppSettingsService : IAppSettingsService
    {
        private string _database =
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}{Path.DirectorySeparatorChar}OpemExamSuite.db";

        private static AppSettingsService _appSettingsService;

        public static AppSettingsService Instance
        {
            get
            {
                if (_appSettingsService != null) return _appSettingsService;
                _appSettingsService = new AppSettingsService();
                return _appSettingsService;
            }
        }

        public void Add(AppSetting settings, AppSettingsType type)
        {
            using (var db = new LiteDatabase(_database))
            {
                var collection = db.GetCollection<AppSetting>(GetTableNameFromType(type));
                var record = collection.FindOne(x => x.FilePath == settings.FilePath);
                if (record != null)
                {
                    return;
                }

                collection.Insert(settings);
            }
        }

        public void Remove(string filePath, AppSettingsType type)
        {
            using (var db = new LiteDatabase(_database))
            {
                var collection = db.GetCollection<AppSetting>(GetTableNameFromType(type));
                collection.Delete(x => x.FilePath == filePath);
            }
        }

        public void Clear(AppSettingsType type)
        {
            using (var db = new LiteDatabase(_database))
            {
                db.DropCollection(GetTableNameFromType(type));
            }
        }

        public IEnumerable<AppSetting> GetAll(AppSettingsType type)
        {
            using (var db = new LiteDatabase(_database))
            {
                var collection = db.GetCollection<AppSetting>(GetTableNameFromType(type));
                return collection.FindAll();
            }
        }

        private string GetTableNameFromType(AppSettingsType type)
        {
            switch (type)
            {
                case AppSettingsType.Creator:
                    return "CreatorSettings";
                case AppSettingsType.Simulator:
                    return "SimulatorSettings";
                default:
                    return "OtherSettings";
            }
        }
    }
}