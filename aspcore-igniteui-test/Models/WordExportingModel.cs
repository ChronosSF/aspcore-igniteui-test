using Infragistics.Documents.Word;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IgniteUI.AspCore.Test.Models
{
    class WordExportingModel
    {
        #region Members

        #region Private Members

        private WordDocumentWriter wordWriter;
        private MemoryStream memStream;

        #endregion

        #endregion

        #region Constructor

        public WordExportingModel()
        {
            memStream = new MemoryStream();
            wordWriter = WordDocumentWriter.Create(memStream);
        }

        public WordExportingModel(MemoryStream ms)
        {
            memStream = ms;
            wordWriter = WordDocumentWriter.Create(memStream);
        }

        #endregion

        #region Properties

        public MemoryStream WordMemoryStream
        {
            get
            {
                return this.memStream;
            }
        }

        public WordDocumentWriter ExcelWorkbook
        {
            get
            {
                return this.wordWriter;
            }
        }

        #endregion

        #region Public Methods

        public void PopulateWordDocument(List<Order> data)
        {
            wordWriter.StartDocument();
            wordWriter.StartParagraph();
            wordWriter.AddTextRun("Congratulations! You have successfully exported igGrid data to Word!");
            wordWriter.EndParagraph();

            // Create the table properties
            TableProperties tableProps = wordWriter.CreateTableProperties();
            tableProps.Alignment = ParagraphAlignment.Center;
            tableProps.BorderProperties.Color = Color.Black;
            tableProps.BorderProperties.Style = TableBorderStyle.Single;

            // Create the table header row properties
            TableRowProperties headerRowProps = wordWriter.CreateTableRowProperties();
            headerRowProps.IsHeaderRow = true;

            // Create the table header cell properties
            TableCellProperties headerCellProps = wordWriter.CreateTableCellProperties();
            headerCellProps.BackColor = Color.DarkGray;
            headerCellProps.TextDirection = TableCellTextDirection.LeftToRightTopToBottom;

            // Start a table
            wordWriter.StartTable(2, tableProps);

            // Start a row and apply it the header row properties
            wordWriter.StartTableRow(headerRowProps);

            // Create header row
            AddTableCell(wordWriter, headerCellProps, "Order ID");
            AddTableCell(wordWriter, headerCellProps, "Contact Name");
            AddTableCell(wordWriter, headerCellProps, "Shipping Address");
            AddTableCell(wordWriter, headerCellProps, "Order Date");

            // End the header row
            wordWriter.EndTableRow();

            // Create the table's content cell properties
            TableCellProperties contentCellProps = wordWriter.CreateTableCellProperties();
            contentCellProps.BackColor = Color.FromArgb(235, 235, 235);
            contentCellProps.PreferredWidthAsPercentage = 0.5f;

            // Iterate through the grid rows to extract the content data
            int i = 0;
            foreach (Order order in data)
            {
                // Alternating row style
                if (i % 2 == 1)
                    contentCellProps.BackColor = Color.White;
                else
                    contentCellProps.BackColor = Color.FromArgb(235, 235, 235);

                wordWriter.StartTableRow();
                AddTableCell(wordWriter, contentCellProps, order.OrderID.ToString());
                AddTableCell(wordWriter, contentCellProps, order.ContactName);
                AddTableCell(wordWriter, contentCellProps, order.ShipAddress);
                AddTableCell(wordWriter, contentCellProps, order.OrderDate != null ? string.Format("{0:d}", order.OrderDate) : "");
                wordWriter.EndTableRow();
                i++;
            }

            // End the table
            wordWriter.EndTable();

            // End the document
            wordWriter.EndDocument();
            wordWriter.Close();
        }

        #endregion

        #region Private Methods

        private void AddTableCell(WordDocumentWriter documentWriter, TableCellProperties cellProperties, string cellText)
        {
            // Start a Cell
            documentWriter.StartTableCell(cellProperties);

            // Start a Paragraph and add a text run to the cell
            documentWriter.StartParagraph();
            documentWriter.AddTextRun(cellText);
            documentWriter.EndParagraph();

            // End the Cell
            documentWriter.EndTableCell();
        }

        #endregion

    }

}
