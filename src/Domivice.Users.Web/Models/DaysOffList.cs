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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Domivice.Users.Web.Models;

/// <summary>
///     Carries a paginated list of users
/// </summary>
[DataContract]
public class DaysOffList : IEquatable<DaysOffList>
{
    /// <summary>
    ///     The days off list
    /// </summary>
    /// <value>The days off list</value>
    [Required]
    [DataMember(Name = "data", EmitDefaultValue = false)]
    public List<DayOff> Data { get; set; }

    /// <summary>
    ///     The URL to access the next page
    /// </summary>
    /// <value>The URL to access the next page</value>
    [DataMember(Name = "nextPage", EmitDefaultValue = false)]
    public string NextPage { get; set; }

    /// <summary>
    ///     The page count
    /// </summary>
    /// <value>The page count</value>
    [Required]
    [DataMember(Name = "pageCount", EmitDefaultValue = true)]
    public int PageCount { get; set; }

    /// <summary>
    ///     The URL to access the previous page
    /// </summary>
    /// <value>The URL to access the previous page</value>
    [DataMember(Name = "previousPage", EmitDefaultValue = false)]
    public string PreviousPage { get; set; }

    /// <summary>
    ///     The total items count
    /// </summary>
    /// <value>The total items count</value>
    [Required]
    [DataMember(Name = "totalItemsCount", EmitDefaultValue = true)]
    public int TotalItemsCount { get; set; }

    /// <summary>
    ///     Returns true if DaysOffList instances are equal
    /// </summary>
    /// <param name="other">Instance of DaysOffList to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(DaysOffList other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return
            (
                Data == other.Data ||
                (Data != null &&
                 other.Data != null &&
                 Data.SequenceEqual(other.Data))
            ) &&
            (
                NextPage == other.NextPage ||
                (NextPage != null &&
                 NextPage.Equals(other.NextPage))
            ) &&
            (
                PageCount == other.PageCount ||
                PageCount.Equals(other.PageCount)
            ) &&
            (
                PreviousPage == other.PreviousPage ||
                (PreviousPage != null &&
                 PreviousPage.Equals(other.PreviousPage))
            ) &&
            (
                TotalItemsCount == other.TotalItemsCount ||
                TotalItemsCount.Equals(other.TotalItemsCount)
            );
    }

    /// <summary>
    ///     Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class DaysOffList {\n");
        sb.Append("  Data: ").Append(Data).Append("\n");
        sb.Append("  NextPage: ").Append(NextPage).Append("\n");
        sb.Append("  PageCount: ").Append(PageCount).Append("\n");
        sb.Append("  PreviousPage: ").Append(PreviousPage).Append("\n");
        sb.Append("  TotalItemsCount: ").Append(TotalItemsCount).Append("\n");
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
        return obj.GetType() == GetType() && Equals((DaysOffList)obj);
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
            if (Data != null)
                hashCode = hashCode * 59 + Data.GetHashCode();
            if (NextPage != null)
                hashCode = hashCode * 59 + NextPage.GetHashCode();

            hashCode = hashCode * 59 + PageCount.GetHashCode();
            if (PreviousPage != null)
                hashCode = hashCode * 59 + PreviousPage.GetHashCode();

            hashCode = hashCode * 59 + TotalItemsCount.GetHashCode();
            return hashCode;
        }
    }

    #region Operators

#pragma warning disable 1591

    public static bool operator ==(DaysOffList left, DaysOffList right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(DaysOffList left, DaysOffList right)
    {
        return !Equals(left, right);
    }

#pragma warning restore 1591

    #endregion Operators
}