﻿using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TopPartsElectronics_PS.Helper;

namespace TopPartsElectronics_PS
{
    public partial class BarcodePrint : Form
    {
        DataSetBarCode.BarcodeDataTable _Barcode;
        public BarcodePrint(DataSetBarCode.BarcodeDataTable Barcode)
        {
            _Barcode = Barcode;
            /// Bar code
            InitializeComponent();
        }
        public BarcodePrint()
        {
            InitializeComponent();           
        }
        private void reportViewer1_Load(object sender, EventArgs e)
        {
            try
            {
                string exportOption_excel = "EXCELOPENXML";
                string exportOption_word = "WORDOPENXML";
                RenderingExtension extensionex = reportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOption_excel, StringComparison.CurrentCultureIgnoreCase));
                if (extensionex != null)
                {
                    System.Reflection.FieldInfo info = extensionex.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    info.SetValue(extensionex, false);
                }
                RenderingExtension extensionwd = reportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOption_word, StringComparison.CurrentCultureIgnoreCase));
                if (extensionwd != null)
                {
                    System.Reflection.FieldInfo info = extensionwd.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    info.SetValue(extensionwd, false);
                }              
                reportViewer1.ProcessingMode = ProcessingMode.Local; 
                LocalReport localReport = reportViewer1.LocalReport;
                localReport.ReportPath = "barcode.rdlc";
                localReport.DisplayName = "BC";                
                //  localReport.ReportPath = "PInfoReport.rdlc";
                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "DataSetBarCode";
                reportDataSource.Value = _Barcode;
                //  reportDataSource.Name = "DataSet3";
                reportViewer1.LocalReport.EnableExternalImages = true;
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.ReportEmbeddedResource = "AssestsManagementSystem.ReportPatentinfo.rdlc";
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                reportViewer1.RefreshReport();
            }
            catch(Exception ex)
            {
                throw ex;

            }
        }
        private void BarcodePrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
        private void BarcodePrint_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((FormProductionInput)Owner).print_return();
            this.Close();
        }
    }
}
