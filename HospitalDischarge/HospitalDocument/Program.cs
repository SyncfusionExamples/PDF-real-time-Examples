﻿using HospitalDocument;
using HospitalDocument.Model;



FileStream fs = new FileStream("PatientDischargeSummary.pdf", FileMode.Create);
DischargeModel model = DischargeDataSource.GetDischargeModel();
DischargeDocument document = new DischargeDocument(model);
document.GeneratePdf(fs);
fs.Close();
