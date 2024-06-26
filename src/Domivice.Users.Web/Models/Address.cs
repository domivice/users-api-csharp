/*
 * Users API
 *
 * The users API
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Domivice.Users.Web.Models;

/// <summary>
/// </summary>
[DataContract]
public class Address : IEquatable<Address>
{
    /// <summary>
    ///     The city
    /// </summary>
    /// <value>The city</value>
    [Required]
    [DataMember(Name = "city", EmitDefaultValue = false)]
    public string City { get; set; }

    /// <summary>
    ///     The country
    /// </summary>
    /// <value>The country</value>
    [Required]
    [DataMember(Name = "country", EmitDefaultValue = false)]
    public string Country { get; set; }

    /// <summary>
    ///     The postal code
    /// </summary>
    /// <value>The postal code</value>
    [Required]
    [DataMember(Name = "postalCode", EmitDefaultValue = false)]
    public string PostalCode { get; set; }

    /// <summary>
    ///     The state or province
    /// </summary>
    /// <value>The state or province </value>
    [Required]
    [DataMember(Name = "state", EmitDefaultValue = false)]
    public string State { get; set; }

    /// <summary>
    ///     The street part of the address
    /// </summary>
    /// <value>The street part of the address</value>
    [Required]
    [DataMember(Name = "street", EmitDefaultValue = false)]
    public string Street { get; set; }

    /// <summary>
    ///     Returns true if Address instances are equal
    /// </summary>
    /// <param name="other">Instance of Address to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(Address other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return
            (
                City == other.City ||
                (City != null &&
                 City.Equals(other.City))
            ) &&
            (
                Country == other.Country ||
                (Country != null &&
                 Country.Equals(other.Country))
            ) &&
            (
                PostalCode == other.PostalCode ||
                (PostalCode != null &&
                 PostalCode.Equals(other.PostalCode))
            ) &&
            (
                State == other.State ||
                (State != null &&
                 State.Equals(other.State))
            ) &&
            (
                Street == other.Street ||
                (Street != null &&
                 Street.Equals(other.Street))
            );
    }

    /// <summary>
    ///     Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class Address {\n");
        sb.Append("  City: ").Append(City).Append("\n");
        sb.Append("  Country: ").Append(Country).Append("\n");
        sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
        sb.Append("  State: ").Append(State).Append("\n");
        sb.Append("  Street: ").Append(Street).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    /// <summary>
    ///     Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    /// <summary>
    ///     Returns true if objects are equal
    /// </summary>
    /// <param name="obj">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Address)obj);
    }

    /// <summary>
    ///     Gets the hash code
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 41;
            // Suitable nullity checks etc, of course :)
            if (City != null)
                hashCode = hashCode * 59 + City.GetHashCode();
            if (Country != null)
                hashCode = hashCode * 59 + Country.GetHashCode();
            if (PostalCode != null)
                hashCode = hashCode * 59 + PostalCode.GetHashCode();
            if (State != null)
                hashCode = hashCode * 59 + State.GetHashCode();
            if (Street != null)
                hashCode = hashCode * 59 + Street.GetHashCode();
            return hashCode;
        }
    }

    #region Operators

#pragma warning disable 1591

    public static bool operator ==(Address left, Address right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Address left, Address right)
    {
        return !Equals(left, right);
    }

#pragma warning restore 1591

    #endregion Operators
}