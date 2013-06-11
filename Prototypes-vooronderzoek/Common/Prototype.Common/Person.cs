namespace Prototype.Common {
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Prototype.Common.GenericAnnotations;
    using Prototype.Common.Resources;

    /// <summary>
    /// Represents a person's data
    /// </summary>
    [Serializable]
    public sealed class Person : IEquatable<Person> {
        [Display(ResourceType = typeof(PersonStrings), Name = "UniqueId_Name")]
        [ApplicationGenerated]
        public long UniqueId { get; set; }

        [Display(ResourceType = typeof(PersonStrings), Name = "FullName_Name")]
        [GenericRequired(AllowEmptyStrings = false)]
        [GenericStringLength(50, MinimumLength = 5)]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(PersonStrings), Name = "HouseNumber_Name")]
        [GenericRange(1, 999)]
        public int HouseNumber { get; set; }

        [Display(ResourceType = typeof(PersonStrings), Name = "BirthDate_Name")]
        public DateTime BirthDate { get; set; }

        [ApplicationGenerated]
        [Display(ResourceType = typeof(PersonStrings), Name = "RegistrationDate_Name")]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public Person() {
            this.UniqueId = UniqueIdentifierGenerator<Person>.GetNewId();
            this.RegistrationDate = DateTime.UtcNow;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public Person(long uniqueId, string fullName, int houseNumber, DateTime birthDate, DateTime registrationDate) {
            this.UniqueId = uniqueId;
            this.FullName = fullName;
            this.HouseNumber = houseNumber;
            this.BirthDate = birthDate;
            this.RegistrationDate = registrationDate;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public Person(string fullName, int houseNumber, DateTime birthDate)
                : this() {
            this.FullName = fullName;
            this.HouseNumber = houseNumber;
            this.BirthDate = birthDate;
        }

        public bool Equals(Person other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return other.UniqueId == this.UniqueId;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != typeof(Person)) {
                return false;
            }
            return Equals((Person)obj);
        }

        public override int GetHashCode() {
            return this.UniqueId.GetHashCode();
        }

        public static bool operator ==(Person left, Person right) {
            return Equals(left, right);
        }

        public static bool operator !=(Person left, Person right) {
            return !Equals(left, right);
        }
    }
}