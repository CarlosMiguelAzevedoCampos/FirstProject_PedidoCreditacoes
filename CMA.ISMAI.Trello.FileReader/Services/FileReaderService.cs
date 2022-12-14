using CMA.ISMAI.Core;
using CMA.ISMAI.Logging.Interface;
using CMA.ISMAI.Trello.FileReader.Interfaces;
using CMA.ISMAI.Trello.FileReader.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CMA.ISMAI.Trello.FileReader.Services
{
    public class FileReaderService : IFileReader
    {
        private readonly ILog _log;

        public FileReaderService(ILog log)
        {
            _log = log;
        }

        public string ReturnUserNameForTheCard(string institute, string course, int pageId, string filePath)
        {
            List<FileModel> fileModelList = ReturnFileData(filePath);
            FileModel model = fileModelList.Where(x=>x.InstituteName == institute && x.CourseName == course && x.Page == pageId).FirstOrDefault();
            return model == null ? string.Empty : model.UserName;
        }

        private List<FileModel> ReturnFileData(string filePath)
        {
            List<FileModel> fileModelList = new List<FileModel>();
            try
            {
                var fi = new FileInfo(filePath);
                if (fi.Exists)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(fi))
                    {
                        for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                        {
                            var totalRows = package.Workbook.Worksheets[i].Dimension?.Rows;
                            for (int j = 1; j <= totalRows.Value; j++)
                            {
                                try
                                {
                                    fileModelList.Add(new FileModel(package.Workbook.Worksheets[i].Cells[j, 1].Value.ToString(),
                                         package.Workbook.Worksheets[i].Cells[j, 2].Value.ToString(),
                                         package.Workbook.Worksheets[i].Cells[j, 3].Value.ToString(), i));
                                }
                                catch (Exception ex)
                                {
                                    _log.Info(string.Format("{0}, This error's can happend when empty cells are in the excel..", ex.ToString()));
                                }
                            }
                        }
                    }
                    return fileModelList;
                }
                _log.Info("File not found...");
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.ToString());
            }
            return new List<FileModel>();
        }
    }
}