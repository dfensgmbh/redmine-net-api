﻿/*
   Copyright 2011 - 2016 Adrian Popescu.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Redmine.Net.Api.Extensions;
using Redmine.Net.Api.Internals;

namespace Redmine.Net.Api.Types
{
    /// <summary>
    /// Availability 2.1
    /// </summary>
    [XmlRoot(RedmineKeys.GROUP)]
    public class Group : IdentifiableName, IEquatable<Group>
    {
        /// <summary>
        /// Represents the group's users.
        /// </summary>
        [XmlArray(RedmineKeys.USERS)]
        [XmlArrayItem(RedmineKeys.USER)]
        public List<GroupUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the custom fields.
        /// </summary>
        /// <value>The custom fields.</value>
        [XmlArray(RedmineKeys.CUSTOM_FIELDS)]
        [XmlArrayItem(RedmineKeys.CUSTOM_FIELD)]
        public IList<IssueCustomField> CustomFields { get; set; }

        /// <summary>
        /// Gets or sets the custom fields.
        /// </summary>
        /// <value>The custom fields.</value>
        [XmlArray(RedmineKeys.MEMBERSHIPS)]
        [XmlArrayItem(RedmineKeys.MEMBERSHIP)]
        public IList<Membership> Memberships { get; set; }

        #region Implementation of IXmlSerializable

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized. </param>
        public override void ReadXml(XmlReader reader)
        {
            reader.Read();
            while (!reader.EOF)
            {
                if (reader.IsEmptyElement && !reader.HasAttributes)
                {
                    reader.Read();
                    continue;
                }

                switch (reader.Name)
                {
                    case RedmineKeys.ID: Id = reader.ReadElementContentAsInt(); break;

                    case RedmineKeys.NAME: Name = reader.ReadElementContentAsString(); break;

                    case RedmineKeys.USERS: Users = reader.ReadElementContentAsCollection<GroupUser>(); break;

                    case RedmineKeys.CUSTOM_FIELDS: CustomFields = reader.ReadElementContentAsCollection<IssueCustomField>(); break;

                    case RedmineKeys.MEMBERSHIPS: Memberships = reader.ReadElementContentAsCollection<Membership>(); break;

                    default: reader.Read(); break;
                }
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized. </param>
        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(RedmineKeys.NAME, Name);
            writer.WriteArrayIds(Users, RedmineKeys.USER_IDS, typeof(int), GetGroupUserId);
            //            if (Users == null) return;
            //
            //            writer.WriteStartElement(RedmineKeys.USER_IDS);
            //            writer.WriteAttributeString("type", "array");
            //            foreach (var userId in Users)
            //            {
            //                new XmlSerializer(typeof(int)).Serialize(writer, userId.Id);
            //            }
            //            writer.WriteEndElement();
        }

        #endregion

        #region Implementation of IEquatable<Group>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Group other)
        {
            if (other == null) return false;
            return Id == other.Id
                && Name == other.Name
                && (Users != null ? Users.Equals<GroupUser>(other.Users) : other.Users == null)
                && (CustomFields != null ? CustomFields.Equals<IssueCustomField>(other.CustomFields) : other.CustomFields == null)
                && (Memberships != null ? Memberships.Equals<Membership>(other.Memberships) : other.Memberships == null);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as Group);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = Utils.GetHashCode(Id, hashCode);
                hashCode = Utils.GetHashCode(Name, hashCode);
                hashCode = Utils.GetHashCode(Users, hashCode);
                hashCode = Utils.GetHashCode(CustomFields, hashCode);
                hashCode = Utils.GetHashCode(Memberships, hashCode);
                return hashCode;
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[Group: Id={0}, Name={1}, Users={2}, CustomFields={3}, Memberships={4}]", Id, Name, Users, CustomFields, Memberships);
        }

        public int GetGroupUserId(object gu)
        {
            return ((GroupUser)gu).Id;
        }
    }

    
}