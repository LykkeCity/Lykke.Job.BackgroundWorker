﻿namespace Lykke.Job.BackgroundWorker.Services.KycCheckService
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class AccessException
    {

        private string messageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationProfile))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PersonProfile))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class Profile
    {

        private string idField;

        private string[] matchingLegalCategoriesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("legalCategory", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] matchingLegalCategories
        {
            get
            {
                return this.matchingLegalCategoriesField;
            }
            set
            {
                this.matchingLegalCategoriesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class OrganisationProfile : Profile
    {

        private string nameField;

        private string[] residencesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("residence", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] residences
        {
            get
            {
                return this.residencesField;
            }
            set
            {
                this.residencesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class PersonProfile : Profile
    {

        private string nameField;

        private string[] citizenshipsField;

        private string[] residencesField;

        private IncompleteDate[] datesOfBirthField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("citizenship", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] citizenships
        {
            get
            {
                return this.citizenshipsField;
            }
            set
            {
                this.citizenshipsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("residence", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] residences
        {
            get
            {
                return this.residencesField;
            }
            set
            {
                this.residencesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("dateOfBirth", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public IncompleteDate[] datesOfBirth
        {
            get
            {
                return this.datesOfBirthField;
            }
            set
            {
                this.datesOfBirthField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class IncompleteDate
    {

        private int yearField;

        private int monthField;

        private int dayField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public int year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public int month
        {
            get
            {
                return this.monthField;
            }
            set
            {
                this.monthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public int day
        {
            get
            {
                return this.dayField;
            }
            set
            {
                this.dayField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationCheckResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PersonCheckResponse))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class CheckResponse
    {

        private long verificationIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public long verificationId
        {
            get
            {
                return this.verificationIdField;
            }
            set
            {
                this.verificationIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class OrganisationCheckResponse : CheckResponse
    {

        private OrganisationProfile[] organisationProfilesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("organisationProfile", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public OrganisationProfile[] organisationProfiles
        {
            get
            {
                return this.organisationProfilesField;
            }
            set
            {
                this.organisationProfilesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class PersonCheckResponse : CheckResponse
    {

        private PersonProfile[] personProfilesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("personProfile", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public PersonProfile[] personProfiles
        {
            get
            {
                return this.personProfilesField;
            }
            set
            {
                this.personProfilesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class PersonName
    {

        private string firstNameField;

        private string lastNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string firstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string lastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationCheckData))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PersonCheckData))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class CheckData
    {

        private string customerIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string customerId
        {
            get
            {
                return this.customerIdField;
            }
            set
            {
                this.customerIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class OrganisationCheckData : CheckData
    {

        private string[] namesField;

        private string[] residencesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("name", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] names
        {
            get
            {
                return this.namesField;
            }
            set
            {
                this.namesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("residence", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] residences
        {
            get
            {
                return this.residencesField;
            }
            set
            {
                this.residencesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cpm.eurospider.com/crif")]
    public partial class PersonCheckData : CheckData
    {

        private PersonName[] namesField;

        private string[] citizenshipsField;

        private string[] residencesField;

        private IncompleteDate[] datesOfBirthField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("name", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public PersonName[] names
        {
            get
            {
                return this.namesField;
            }
            set
            {
                this.namesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("citizenship", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] citizenships
        {
            get
            {
                return this.citizenshipsField;
            }
            set
            {
                this.citizenshipsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("residence", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] residences
        {
            get
            {
                return this.residencesField;
            }
            set
            {
                this.residencesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("dateOfBirth", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public IncompleteDate[] datesOfBirth
        {
            get
            {
                return this.datesOfBirthField;
            }
            set
            {
                this.datesOfBirthField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://cpm.eurospider.com/crif", ConfigurationName = "access")]
    public interface access
    {

        [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
        [System.ServiceModel.FaultContractAttribute(typeof(AccessException), Action = "", Name = "AccessException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Profile))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CheckResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CheckData))]
        System.Threading.Tasks.Task<checkPersonResponse> checkPersonAsync(checkPerson request);

        [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
        [System.ServiceModel.FaultContractAttribute(typeof(AccessException), Action = "", Name = "AccessException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Profile))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CheckResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CheckData))]
        System.Threading.Tasks.Task<listLegalCategoriesResponse> listLegalCategoriesAsync(listLegalCategories request);

        [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
        [System.ServiceModel.FaultContractAttribute(typeof(AccessException), Action = "", Name = "AccessException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Profile))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CheckResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CheckData))]
        System.Threading.Tasks.Task<checkOrganisationResponse> checkOrganisationAsync(checkOrganisation request);

        [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
        [System.ServiceModel.FaultContractAttribute(typeof(AccessException), Action = "", Name = "AccessException")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Profile))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CheckResponse))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CheckData))]
        System.Threading.Tasks.Task<listCountryCodesResponse> listCountryCodesAsync(listCountryCodes request);
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "checkPerson", WrapperNamespace = "http://cpm.eurospider.com/crif", IsWrapped = true)]
    public partial class checkPerson
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://cpm.eurospider.com/crif", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PersonCheckData data;

        public checkPerson()
        {
        }

        public checkPerson(PersonCheckData data)
        {
            this.data = data;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "checkPersonResponse", WrapperNamespace = "http://cpm.eurospider.com/crif", IsWrapped = true)]
    public partial class checkPersonResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://cpm.eurospider.com/crif", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PersonCheckResponse @return;

        public checkPersonResponse()
        {
        }

        public checkPersonResponse(PersonCheckResponse @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "listLegalCategories", WrapperNamespace = "http://cpm.eurospider.com/crif", IsWrapped = true)]
    public partial class listLegalCategories
    {

        public listLegalCategories()
        {
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "listLegalCategoriesResponse", WrapperNamespace = "http://cpm.eurospider.com/crif", IsWrapped = true)]
    public partial class listLegalCategoriesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://cpm.eurospider.com/crif", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] @return;

        public listLegalCategoriesResponse()
        {
        }

        public listLegalCategoriesResponse(string[] @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "checkOrganisation", WrapperNamespace = "http://cpm.eurospider.com/crif", IsWrapped = true)]
    public partial class checkOrganisation
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://cpm.eurospider.com/crif", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OrganisationCheckData data;

        public checkOrganisation()
        {
        }

        public checkOrganisation(OrganisationCheckData data)
        {
            this.data = data;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "checkOrganisationResponse", WrapperNamespace = "http://cpm.eurospider.com/crif", IsWrapped = true)]
    public partial class checkOrganisationResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://cpm.eurospider.com/crif", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OrganisationCheckResponse @return;

        public checkOrganisationResponse()
        {
        }

        public checkOrganisationResponse(OrganisationCheckResponse @return)
        {
            this.@return = @return;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "listCountryCodes", WrapperNamespace = "http://cpm.eurospider.com/crif", IsWrapped = true)]
    public partial class listCountryCodes
    {

        public listCountryCodes()
        {
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "listCountryCodesResponse", WrapperNamespace = "http://cpm.eurospider.com/crif", IsWrapped = true)]
    public partial class listCountryCodesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://cpm.eurospider.com/crif", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] @return;

        public listCountryCodesResponse()
        {
        }

        public listCountryCodesResponse(string[] @return)
        {
            this.@return = @return;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    public interface accessChannel : access, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "0.5.0.0")]
    public partial class accessClient : System.ServiceModel.ClientBase<access>, access
    {

        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);

        public accessClient() :
            base(accessClient.GetDefaultBinding(), accessClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.access.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public accessClient(EndpointConfiguration endpointConfiguration) :
            base(accessClient.GetBindingForEndpoint(endpointConfiguration), accessClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public accessClient(EndpointConfiguration endpointConfiguration, string remoteAddress) :
            base(accessClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public accessClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) :
            base(accessClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }

        public accessClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<checkPersonResponse> access.checkPersonAsync(checkPerson request)
        {
            return base.Channel.checkPersonAsync(request);
        }

        public System.Threading.Tasks.Task<checkPersonResponse> checkPersonAsync(PersonCheckData data)
        {
            checkPerson inValue = new checkPerson();
            inValue.data = data;
            return ((access)(this)).checkPersonAsync(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<listLegalCategoriesResponse> access.listLegalCategoriesAsync(listLegalCategories request)
        {
            return base.Channel.listLegalCategoriesAsync(request);
        }

        public System.Threading.Tasks.Task<listLegalCategoriesResponse> listLegalCategoriesAsync()
        {
            listLegalCategories inValue = new listLegalCategories();
            return ((access)(this)).listLegalCategoriesAsync(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<checkOrganisationResponse> access.checkOrganisationAsync(checkOrganisation request)
        {
            return base.Channel.checkOrganisationAsync(request);
        }

        public System.Threading.Tasks.Task<checkOrganisationResponse> checkOrganisationAsync(OrganisationCheckData data)
        {
            checkOrganisation inValue = new checkOrganisation();
            inValue.data = data;
            return ((access)(this)).checkOrganisationAsync(inValue);
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<listCountryCodesResponse> access.listCountryCodesAsync(listCountryCodes request)
        {
            return base.Channel.listCountryCodesAsync(request);
        }

        public System.Threading.Tasks.Task<listCountryCodesResponse> listCountryCodesAsync()
        {
            listCountryCodes inValue = new listCountryCodes();
            return ((access)(this)).listCountryCodesAsync(inValue);
        }

        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }

        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }

        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.access))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }

        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.access))
            {
                return new System.ServiceModel.EndpointAddress("https://ecpm.eurospider.com/amag-crif-test/access");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }

        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return accessClient.GetBindingForEndpoint(EndpointConfiguration.access);
        }

        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return accessClient.GetEndpointAddress(EndpointConfiguration.access);
        }

        public enum EndpointConfiguration
        {

            access,
        }
    }

}