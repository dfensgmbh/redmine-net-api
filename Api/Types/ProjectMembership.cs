﻿/*
   Copyright 2011 Dorin Huzum, Adrian Popescu.

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
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Redmine.Net.Api.Types
{
    public class ProjectMembership : Identifiable<ProjectMembership>, IEquatable<ProjectMembership>, IXmlSerializable
    {
        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        [XmlElement("project")]
        public IdentifiableName Project { get; set; }

        [XmlElement("user")]
        public IdentifiableName User { get; set; }

        [XmlElement("group")]
        public IdentifiableName Group { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlArray("roles")]
        [XmlArrayItem("role")]
        public List<MembershipRole> Roles { get; set; }

        public bool Equals(ProjectMembership other)
        {
            if (other == null) return false;
            return (Id == other.Id && Project == other.Project && Roles == other.Roles && User == other.User && Group == other.Group);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
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
                    case "id": Id = reader.ReadElementContentAsInt(); break;

                    case "project": Project = new IdentifiableName(reader); break;

                    case "user": User = new IdentifiableName(reader); break;

                    case "group": Group = new IdentifiableName(reader); break;

                    case "roles": Roles = reader.ReadElementContentAsCollection<MembershipRole>(); break;

                    default: reader.Read(); break;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {

        }
    }
}