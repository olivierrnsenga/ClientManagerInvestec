﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ClientManager.Core.Processor;

namespace ClientManager.Core.Validation
{
    public class UniqueMobileNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var clientRequestProcessor =
                (IClientRequestProcessor)validationContext.GetService(typeof(IClientRequestProcessor));

            if (clientRequestProcessor == null)
                throw new InvalidOperationException("IClientRequestProcessor not available in validation context.");

            if (value is string mobileNumber)
            {
                var currentEntityId = (int?)validationContext.ObjectInstance.GetType().GetProperty("Id")?.GetValue(validationContext.ObjectInstance);

                var allClients = clientRequestProcessor.GetAllClients();

                if (currentEntityId.HasValue)
                    allClients = allClients.Where(c => c.Id != currentEntityId);

                if (allClients.Any(c => c.MobileNumber == mobileNumber))
                    return new ValidationResult("Mobile number already exists.");
            }

            return ValidationResult.Success;
        }
    }
}