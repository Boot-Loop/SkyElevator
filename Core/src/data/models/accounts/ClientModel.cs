﻿using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Models
{
	[Serializable]
	public class ClientModel : Model
	{
		public IntergerField	id			{ get; set; } = new IntergerField(		name: "id",			is_required: true);
		public TextField		name		{ get; set; } = new TextField(			name: "name",		max_length:40, is_required: true);
		public TextField		address		{ get; set; } = new TextField(			name: "address"	,	max_length:50);
		public EmailField		email		{ get; set; } = new EmailField(			name: "email"		);
		public TextField		company		{ get; set; } = new TextField(			name: "company",	max_length:40);
		public TextField		position	{ get; set; } = new TextField(			name: "position",	max_length:40);
		public NICField			nic			{ get; set; } = new NICField(			name: "nic",		max_length:10);
		public PhoneNumberField telephone	{ get; set; } = new PhoneNumberField(	name: "telephone",	max_length:20);
		public WebSiteField		website		{ get; set; } = new WebSiteField(		name: "website",	max_length:40);

		public ClientModel() { }
		public ClientModel(string name) { this.name.value = name; }

		/* overrides */
		override public string ToString()				=> this.name.value;
		override public ModelType getType()				=> ModelType.MODEL_CLIENT;
		override public bool matchPK(object pk)			=> id.value == Convert.ToInt64(pk);
		override public object getPK()					=> id.value;
		override public void saveUpdates()				=> Application.singleton.clients_file.save();

		override public void saveNew() {
			var file = Application.singleton.clients_file;
			if (file.data is null) throw new Exception( "did you call Application.singleton.initialize()" );
			file.data.clients.Add(this);
			file.save();
		}
		public override void validateRelation() { }

		public override void delete() {
			var file = Application.singleton.clients_file;
			if (file.data is null) throw new Exception( "did you call Application.singleton.initialize()" );
			if (file.data.clients.Contains(this)) {
				file.data.clients.Remove(this); file.save();
			}
			else Logger.logger.logWarning( "trying to delete a client -> was not in clients file" );
		}
	}
}
