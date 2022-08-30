##### Example: LeaseAgreement

# Purpose
   LeaseAgreement project is an example showing how to generate an Lease Agreement Draft Form document using the PDF library. This example demonstrates how to create a page document that contains paragraphs, and content with various styling. 

The example source is available in [repo](https://github.com/SyncfusionExamples/PDF-real-time-Examples/tree/EJDOTNETCORE-3951/LeaseAgreementProblem/LeaseAgreement).

# Prerequisites
1) **Visual Studio 2017** or above is installed.
   To install a community version of Visual Studio use the following link: https://visualstudio.microsoft.com/vs/community/
   Please make sure that the way you are going to use Visual Studio is allowed by the community license. You may need to buy Standard or Professional Edition.

2) **.NET Core SDK 2.1** or above is installed.
   To install the framework use the following link: https://dotnet.microsoft.com/download

# Description

### Output file
The example creates the file **LeaseAgreement.pdf** in the output **bin/(debug|release)/net6.0** folder.


# Writing the source code

#### 1. Create new console application.
1.1.	Run Visual Studio  
1.2.	File -> Create -> Console Application (.Net Core)

#### 2. Modify class Program.
2.1. Set the path to the output PDF-file In the **Main()** function:

```c#
    FileStream fs = new FileStream("LeaseAgreement.pdf", FileMode.Create);
```
2.2. Call the **GetMonthLeaseDetails()** method  for the LeaseAgreement generation 

```c#
   LeaseModel model = LeaseDataSource.GetMonthLeaseDetails();
   LeaseDocument document = new LeaseDocument(model);
   document.GeneratePdf(fs);
```
#### 3. Create class for Lease model to get and set LeaseAgreement data

```c#
    public class LeaseModel
    {
        public string Title { get; set; }
		
        public string CompanyName { get; set; }

        public string FormDate { get; set; }

        public Parties Parties { get; set; }

        public LeasePeriod LeasePeriod { get; set; }

        public OtherOccupants OtherOccupants1 { get; set; }

        public OtherOccupants OtherOccupants2 { get; set; }

        public AgreementSignature LessorSignature { get; set; }

        public AgreementSignature LesseeSignature { get; set; }

        public TermsofLease TermsofLease { get; set; }
    }
```
	
3.1. Create class for Parties details to get and set Parties details

```c#
     public class Parties
    {
        public string Title { get; set; }
		
        public string Lessor { get; set; }
		
        public string Lessee { get; set; }
		
        public string LeaseTerm { get; set; }

        public PropertyDetails PropertyDetails { get; set; }

        public DateTime PaymentPeriod { get; set; }

    }
```

3.2. Create Property Details class for to get and set PropertyDetails

```c#
    public class PropertyDetails
    {
        public string PlatNo { get; set; }
		
        public string Street { get; set; }
		
        public string City { get; set; }
		
        public int PinCode { get; set; }
    }
```
3.3. Create Lease Period class for to get and set Lease Period details

```c#
   public class LeasePeriod
    {
        public string Title { get; set; }
		
        public string LeaseFrom { get; set; }
		
        public string LeaseUntil { get; set; }

        public float SecurityDebitAmount { get; set; }
		
        public float MonthlyLeaseAmount { get; set; }
    }
```
3.4. Create Other Occupants class for to get and set Other Occupants details

```c#
   public class OtherOccupants
    {
        public string Title { get; set; }
		
        public string Name { get; set; }
		
        public string RelationShip { get; set; }
		
        public string DateofBirth { get; set; }
    }
```
3.5. Create Agreement Signature class for to get and set Agreement Signature details

```c#
 public class AgreementSignature
    {
        public string Signature { get; set; }
    }
```
3.6. Create Terms of Lease class for to get and set Terms of Lease details

```c#
   public class TermsofLease
    {
        public string Title { get; set; }
		
        public string UseandOccupancy { get; set; }
		
        public string AssignmentandSublease { get; set; }
		
        public string Rent { get; set; }
		
        public string Abandonment { get; set; }
		
        public string NecessaryExpenses { get; set; }
		
        public string JudicialAction { get; set; }
		
        public string ImprovementPremises { get; set; }
		
        public string Pets { get; set; }

    }
```

#### 4. Create class for Lease data source to get and set Lease details.	

```c#
   internal class LeaseDataSource
    {
        public static LeaseModel GetMonthLeaseDetails()
        {
            return new LeaseModel
            {
                Title = "Month to Month Lease Agreement",
                CompanyName = "ABZ Company",
                FormDate = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                Parties = GeneratePartiesDetails(),
                LeasePeriod = GenerateLeasePeriod(),
                OtherOccupants1 = GenerateOtherOccupants1(),
                OtherOccupants2 = GenerateOtherOccupants2(),
                LesseeSignature = GenerateAgreementSignature1(),
                LessorSignature = GenerateAgreementSignature2(),
                TermsofLease = GenerateTermsofLeaseDetails(),
            };
        }
        private static Parties GeneratePartiesDetails()
        {
            return new Parties
            {
                Title = "This Agreement is Executed By and Between the Parties:",
                Lessor = "Elvis Peckham",
                Lessee = "Torrin Adrienne",
                LeaseTerm = "Integer",
                PaymentPeriod = new DateTime(2018, 07, 25),
                PropertyDetails = GeneratePropertyDetails(),
            };
        }
        private static PropertyDetails GeneratePropertyDetails()
        {
            return new PropertyDetails
            {
                PlatNo = "64432 Clarendon Alley,",
                Street = "9929 Almo PaWashingto,",
                City = "District o,",
                PinCode = 20062,
            };
        }

        private static LeasePeriod GenerateLeasePeriod()
        {
            return new LeasePeriod
            {
                Title = "Lease Period",
                LeaseFrom = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                LeaseUntil = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                MonthlyLeaseAmount = 1200.00f,
                SecurityDebitAmount = 890.00f,
            };
        }

        private static OtherOccupants GenerateOtherOccupants1()
        {
            return new OtherOccupants
            {
                Title = "Names of Other Occupants 1",
                Name = "Dannel Kreuzer",
                DateofBirth =DateTime.UtcNow.ToString("dddd,2000,08,02"),
                RelationShip = "Intege",
            };
        }
        private static OtherOccupants GenerateOtherOccupants2()
        {
            return new OtherOccupants
            {
                Title = "Names of Other Occupants 2",
                Name = "Raja",
                DateofBirth = DateTime.UtcNow.ToString("dddd,1998,08,02"),
                RelationShip = "Maecen",
            };
        }
        private static AgreementSignature GenerateAgreementSignature1()
        {
            return new AgreementSignature
            {
                Signature = " ",
            };
        }

        private static AgreementSignature GenerateAgreementSignature2()
        {
            return new AgreementSignature
            {
                Signature = " ",
            };
        }
        private static TermsofLease GenerateTermsofLeaseDetails()
        {
            string json = File.ReadAllText(@"..\..\..\Data\StringofData.json");
            TermsofLease Data = JsonConvert.DeserializeObject<TermsofLease>(json);
            return new TermsofLease
            {
                Title = "Terms of Lease",
                Abandonment = Data.Abandonment,
                AssignmentandSublease = Data.AssignmentandSublease,
                JudicialAction = Data.JudicialAction,
                ImprovementPremises = Data.ImprovementPremises,
                Rent = Data.Rent,
                NecessaryExpenses = Data.NecessaryExpenses,
                Pets = Data.Pets,
                UseandOccupancy = Data.UseandOccupancy,
            };
        }

    }
```

#### 5. Create the LeaseDocument class which will contain the method to build the document structure

```c#
public class LeaseDocument
```
5.1. Create ComposeDate() method

Using this method we will draw the current date of right side.

```c#
 public PdfLayoutResult ComposeDate()
        {
            SizeF fontSize = contentFont.MeasureString(model.FormDate);
            float sizeX = clientSize.Width - fontSize.Width;
            var result = new PdfTextElement(model.FormDate, contentFont).Draw(currentPage, new PointF(sizeX, 10));
            return result;
        }
```
5.2. Create ComposeParties() method 

This method is used to draw the parties details,property details and terms

```c#
         public PdfLayoutResult ComposeParties(RectangleF bounds)
        {
            float y = bounds.Bottom + margin;
            float x = (clientSize.Width / 2) ;
            RectangleF halfBounds = new RectangleF(smallTextMargin, smallTextMargin, (clientSize.Width / 2), (clientSize.Height / 2));
            PdfBrush brush = PdfBrushes.LightGreen;
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, y, clientSize.Width, 20));

            var result = new PdfTextElement(model.Parties.Title, titleFont).Draw(currentPage, new RectangleF(Padding, y+ Padding,clientSize.Width,0));
            result = new PdfTextElement("Lessor / Landlord", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Lessee / Tenant", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.Parties.Lessor, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.Parties.Lessee, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Property subject to lease:", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Lease Term", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));


            string address = model.Parties.PropertyDetails.PlatNo + " " + model.Parties.PropertyDetails.Street + "\n" + model.Parties.PropertyDetails.City + " " + model.Parties.PropertyDetails.PinCode;
            result = new PdfTextElement(address, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + 10, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.Parties.LeaseTerm, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Date payment period on every month", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement($"{model.Parties.PaymentPeriod:d}", contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            return result;
        }
```

5.3. create ComposeLeasePeriod() method.

This method is used to draw the lease periods and amount details

```c#
       public PdfLayoutResult ComposeLeasePeriod(RectangleF bounds)
        {
            float y = bounds.Bottom + margin;
            float secondHalf = (clientSize.Width / 4);
            float thirdHalf = (clientSize.Width)/2; 
            float fourthHalf = (clientSize.Width /2)+100;  
            PdfBrush brush = PdfBrushes.LightGreen;
            RectangleF quaterBounds = new RectangleF(10, 10, (clientSize.Width / 4), (clientSize.Height / 4));
            
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, y, clientSize.Width , 20));
            var result = new PdfTextElement(model.LeasePeriod.Title, titleFont).Draw(currentPage, new RectangleF(Padding, y+ Padding, clientSize.Width, 0));
            result = new PdfTextElement("Lease From", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement("Lease Until", titleFont).Draw(currentPage, new RectangleF(thirdHalf, result.Bounds.Y, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement(model.LeasePeriod.LeaseFrom, contentFont).Draw(currentPage, new RectangleF(secondHalf, result.Bounds.Y, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement(model.LeasePeriod.LeaseUntil, contentFont).Draw(currentPage, new RectangleF(fourthHalf, result.Bounds.Y, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement("Security Deposit Amount", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement("$ " + model.LeasePeriod.SecurityDebitAmount.ToString(), contentFont).Draw(currentPage, new RectangleF(secondHalf, result.Bounds.Y, quaterBounds.Width, quaterBounds.Height));
            result = new PdfTextElement("Monthly Lease Amount", titleFont).Draw(currentPage, new RectangleF(thirdHalf, result.Bounds.Y, quaterBounds.Width/1.5f, quaterBounds.Height));
            result = new PdfTextElement("$ " + model.LeasePeriod.MonthlyLeaseAmount.ToString(), contentFont).Draw(currentPage, new RectangleF(fourthHalf, result.Bounds.Y, quaterBounds.Width, quaterBounds.Height));
            return result;
        }
```
5.4. create ComposeOtherOccupants() method.

This method is used to draw the occupants, relationships and signature details

```c#
       public PdfLayoutResult ComposeOtherOccupants(RectangleF bounds)
        {
            float y = bounds.Bottom + margin;
            float x = (clientSize.Width / 2);
            RectangleF halfBounds = new RectangleF(10, 10, (clientSize.Width / 2), (clientSize.Height / 2));
            PdfBrush brush = PdfBrushes.LightGreen;
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, y, clientSize.Width - 20, 20));


            var result = new PdfTextElement(model.OtherOccupants1.Title, titleFont).Draw(currentPage, new RectangleF(Padding, y+ Padding,clientSize.Width,0));
            result = new PdfTextElement("Name of other occupant 1", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width,halfBounds.Height));
            result = new PdfTextElement("Name of other occupant 2", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.Name, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.Name, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Relationship", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Relationship", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.RelationShip, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.RelationShip, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));


            result = new PdfTextElement("Date of Birth", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Date of Birth", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants1.DateofBirth, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + smallTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.OtherOccupants2.DateofBirth, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Lessor Signature", titleFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + 100, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("Lessee Signature", titleFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.LessorSignature.Signature, contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom+ largeTextMargin, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement(model.LesseeSignature.Signature, contentFont).Draw(currentPage, new RectangleF(x, result.Bounds.Y, halfBounds.Width, halfBounds.Height));
            result = new PdfTextElement("-----------------------------------------------------------------page break--------------------------------------------------------------", contentFont).Draw(currentPage, new RectangleF(20, result.Bounds.Bottom+30,clientSize.Width,0));
            result = new PdfTextElement(" ", contentFont).Draw(currentPage, new RectangleF(0, result.Bounds.Bottom + largeTextMargin, 0, 0));
            return result;
        }
```
5.5. create ComposeTermsofLease() method.

This method is used to draw the terms and conditions for lease agreement

```c#
       public PdfLayoutResult ComposeTermsofLease(RectangleF bounds)
        {
            float y = bounds.Bottom;
            float x = (clientSize.Width / 2) ;
            float width = (clientSize.Width / 2) - 40;
            PdfBrush brush = PdfBrushes.LightGreen;
            RectangleF rect = new RectangleF(10, 10, (clientSize.Width / 2), (clientSize.Height / 2));
            var result = new PdfTextElement(model.TermsofLease.Title, titleFont).Draw(currentPage, new RectangleF(width, y,clientSize.Width,0));

            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, result.Bounds.Bottom+ 10, rect.Width - 10, 20));
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(x, result.Bounds.Bottom+ 10, rect.Width - 10, 20));
            var leftsideResult = new PdfTextElement("Use and Occupancy", titleFont).Draw(currentPage, new RectangleF(Padding, result.Bounds.Bottom+ smallTextMargin + Padding, rect.Width - 25, rect.Height));
            var rightsideResult = new PdfTextElement("Assignment and Sublease", titleFont).Draw(currentPage, new RectangleF(x+ Padding, leftsideResult.Bounds.Y, rect.Width - 10, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.UseandOccupancy, contentFont).Draw(currentPage, new RectangleF(0, leftsideResult.Bounds.Bottom + smallTextMargin, rect.Width - 10, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.AssignmentandSublease, contentFont).Draw(currentPage, new RectangleF(x+5, leftsideResult.Bounds.Y, rect.Width - 10, rect.Height));

            if(rightsideResult.Bounds.Bottom>leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }

            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, leftsideResult.Bounds.Bottom +30, rect.Width - 10, 20));
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(x, leftsideResult.Bounds.Bottom +30, rect.Width - 10, 20));
            leftsideResult = new PdfTextElement("Rent", titleFont).Draw(currentPage, new RectangleF(Padding, leftsideResult.Bounds.Bottom + largeTextMargin + Padding, rect.Width, rect.Height));
            rightsideResult = new PdfTextElement("Abandonment", titleFont).Draw(currentPage, new RectangleF(x + Padding, leftsideResult.Bounds.Y, rect.Width, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.Rent, contentFont).Draw(currentPage, new RectangleF(0, leftsideResult.Bounds.Bottom + smallTextMargin, rect.Width - 10, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.Abandonment, contentFont).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - 10, rect.Height));

            if (rightsideResult.Bounds.Bottom > leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }

            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, leftsideResult.Bounds.Bottom + 30, rect.Width - 10, 20));
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(x, leftsideResult.Bounds.Bottom + 30, rect.Width - 10, 20));
            leftsideResult = new PdfTextElement("Necessary Expenses", titleFont).Draw(currentPage, new RectangleF(Padding, leftsideResult.Bounds.Bottom + largeTextMargin + Padding, rect.Width, rect.Height));
            rightsideResult = new PdfTextElement("Judicial Action", titleFont).Draw(currentPage, new RectangleF(x + Padding, leftsideResult.Bounds.Y, rect.Width, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.NecessaryExpenses, contentFont).Draw(currentPage, new RectangleF(0, leftsideResult.Bounds.Bottom + smallTextMargin, rect.Width - 10, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.JudicialAction, contentFont).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - 10, rect.Height));

            if (rightsideResult.Bounds.Bottom > leftsideResult.Bounds.Bottom)
            {
                leftsideResult = rightsideResult;
            }

            currentPage.Graphics.DrawRectangle(brush, new RectangleF(0, leftsideResult.Bounds.Bottom + 30, rect.Width - 10, 20));
            currentPage.Graphics.DrawRectangle(brush, new RectangleF(x, leftsideResult.Bounds.Bottom + 30, rect.Width - 10, 20));
            leftsideResult = new PdfTextElement("Improvements to the Premises", titleFont).Draw(currentPage, new RectangleF(Padding, leftsideResult.Bounds.Bottom + largeTextMargin + Padding, rect.Width, rect.Height));
            rightsideResult = new PdfTextElement("Pets", titleFont).Draw(currentPage, new RectangleF(x + Padding, leftsideResult.Bounds.Y, rect.Width, rect.Height));
            leftsideResult = new PdfTextElement(model.TermsofLease.ImprovementPremises, contentFont).Draw(currentPage, new RectangleF(0, leftsideResult.Bounds.Bottom + smallTextMargin, rect.Width - 10, rect.Height));
            rightsideResult = new PdfTextElement(model.TermsofLease.Pets, contentFont).Draw(currentPage, new RectangleF(x, leftsideResult.Bounds.Y, rect.Width - 10, rect.Height));

            return result;
        }
```
#### 6. Generated **PDF file** must look as shown below:
The resulting LeaseAgreement.pdf document can be accessed [here](https://github.com/SyncfusionExamples/PDF-real-time-Examples/blob/EJDOTNETCORE-3951/LeaseAgreementProblem/LeaseAgreement/LeaseAgreement.pdf).
    
