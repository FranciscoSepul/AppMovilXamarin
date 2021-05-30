using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PDRProvBackEnd.Migrations
{
    public partial class Indice : Migration
    {
        private string UserIdAdmin = "2A684CA3-A780-4B0E-9206-DAD309D6ECDF";
        private string UserIdAdministrador = "3A664CA3-A780-4B0E-9206-DAD309D6ECDF";
        private string UserIdClient = "C4E28010-DF49-4D3F-9996-27E9B2AD45F8";
        private string UserNameAdmin = "admin";
        private string UserNameAdministrador = "administrador";
        private string UserNameClient = "client";
        private string PasswordAdmin = "123";
        private string PasswordAdministrador = "123";
        private string PasswordClient = "123";

        private string SupplierId = "C4E28010-DF49-4D3F-9996-27E9B2AD45F4";
        private string SupplierRut = "11111111-1";
        private string SupplierName = "Carlos";
        private string Giro = "Servicios Aeronáuticos";
        private string NombreContact = "Pedro";
        private string PhoneContact = "966666666";
        private string EmailContact = "Carlos@servicioAeronauticos.cl";
        private Boolean hasContact = false;
        private Boolean hasCOutSour = false;
        private Boolean hasEthicalMana = false;
        private Boolean hasCertification = false;
        private Boolean disable = false;

        private string IdIndiceDatos = "2A684CA3-A780-4B0E-9206-DAD309D6ECDD";
        private string IdReason = "1";
        private string IdUserSending = "C4E28010-DF49-4D3F-9996-27E9B2AD45F8";
        private string Title = "Data Inicial ";
        private int TypeContact = 1;
        private Boolean IsRead = false;

        private string UserSupplierId = "C4E28010-DF49-4D3F-9996-27E9B2AD45F9";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(name: "IX_Users_Username", table: "Users", column: "Username", unique: true);
            migrationBuilder.CreateIndex(name: "IX_Users_Email", table: "Users", column: "Email", unique: true);
            migrationBuilder.CreateIndex(name: "IX_Users_Disabled", table: "Users", column: "Disabled", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Users_Expires", table: "Users", column: "Expires", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Users_CreatedAt", table: "Users", column: "CreatedAt", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Users_UpdatedAt", table: "Users", column: "UpdatedAt", unique: false);
            migrationBuilder.CreateIndex(name: "IX_RoleUser_RolesName", table: "RoleUser", column: "RolesName", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Role_Name", table: "Role", column: "Name", unique: false);

            migrationBuilder.CreateIndex(name: "IX_Supplier_Id", table: "Suppliers", column: "Id", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_Rut", table: "Suppliers", column: "Rut", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_Name", table: "Suppliers", column: "NameSupplier", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_Giro", table: "Suppliers", column: "Giro", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_NameContact", table: "Suppliers", column: "NameContact", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_PhoneContact", table: "Suppliers", column: "PhoneContact", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_EmailContact", table: "Suppliers", column: "EmailContact", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_HasContract", table: "Suppliers", column: "HasContract", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_HasOutsourcingService", table: "Suppliers", column: "HasOutsourcingService", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_HasEthicalManagement", table: "Suppliers", column: "HasEthicalManagement", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_HasCertification", table: "Suppliers", column: "HasCertification", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_Disabled", table: "Suppliers", column: "Disabled", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Supplier_CreatedAt", table: "Suppliers", column: "CreatedAt", unique: false);

            migrationBuilder.CreateIndex(name: "IX_Messagecontact_Title", table: "Messagecontacts", column: "Title", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Messagecontact_TypeContact", table: "Messagecontacts", column: "TypeContact", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Messagecontact_IsRead", table: "Messagecontacts", column: "IsRead", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Messagecontact_ResponseToId", table: "Messagecontacts", column: "ResponseToId", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Messagecontact_SendingUserId", table: "Messagecontacts", column: "SendingUserId", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Messagecontact_ReasonContactId", table: "Messagecontacts", column: "ReasonContactId", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Messagecontact_Disabled", table: "Messagecontacts", column: "Disabled", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Messagecontact_CreatedAt", table: "Messagecontacts", column: "CreatedAt", unique: false);
            migrationBuilder.CreateIndex(name: "IX_Messagecontact_UpdatedAt", table: "Messagecontacts", column: "UpdatedAt", unique: false);

            migrationBuilder.CreateIndex(name: "IX_ReasonContact_Id", table: "ReasonContacts", column: "Id", unique: false);
            migrationBuilder.CreateIndex(name: "IX_ReasonContact_Reason", table: "ReasonContacts", column: "Reason", unique: false);
            migrationBuilder.CreateIndex(name: "IX_ReasonContact_ClassificationContact", table: "ReasonContacts", column: "TypeContact", unique: false);

            migrationBuilder.CreateIndex(name: "IX_UserSuppliers_User", table: "UserSuppliers", column: "UserId", unique: true);
            migrationBuilder.CreateIndex(name: "IX_UserSuppliers_Supplier", table: "UserSuppliers", column: "SupplierId", unique: true);
            migrationBuilder.CreateIndex(name: "IX_UserSuppliers_Disabled", table: "UserSuppliers", column: "Disabled", unique: false);
            migrationBuilder.CreateIndex(name: "IX_UserSuppliers_CreatedAt", table: "UserSuppliers", column: "CreatedAt", unique: false);
            migrationBuilder.CreateIndex(name: "IX_UserSuppliers_UpdatedAt", table: "UserSuppliers", column: "UpdatedAt", unique: false);

            byte[] passwordHash, passwordSalt = null;
            Services.UsersService.CreatePasswordHash(this.PasswordAdmin, out passwordHash, out passwordSalt);
            migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "Username", "Email", "PasswordHash","PasswordSalt",
                "Disabled", "Expires", "CreatedAt", "UpdatedAt" },
            values: new object[] { this.UserIdAdmin, this.UserNameAdmin, "admin@pawa.com",
            passwordHash,
            passwordSalt,
            false, null, DateTime.UtcNow, null }); 

            passwordHash = null; passwordSalt = null;
            Services.UsersService.CreatePasswordHash(this.PasswordClient, out passwordHash, out passwordSalt);
            migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "Username", "Email", "PasswordHash","PasswordSalt",
                "Disabled", "Expires", "CreatedAt", "UpdatedAt" },
            values: new object[] { this.UserIdClient, this.UserNameClient, "client@pawa.com",
            passwordHash,
            passwordSalt,
            false, null, DateTime.UtcNow, null });

            passwordHash = null; passwordSalt = null;
            Services.UsersService.CreatePasswordHash(this.PasswordAdministrador, out passwordHash, out passwordSalt);
            migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Id", "Username", "Email", "PasswordHash","PasswordSalt",
                "Disabled", "Expires", "CreatedAt", "UpdatedAt" },
            values: new object[] { this.UserIdAdministrador, this.UserNameAdministrador, "administrador@pawa.com",
            passwordHash,
            passwordSalt,
            false, null, DateTime.UtcNow, null });

            //roles
            migrationBuilder.InsertData(
               table: "Role",
               columns: new[] { "Name" },
               values: new object[] { PDRProvBackEnd.Entities.Roles.SuperAdmin });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Name" },
                values: new object[] { PDRProvBackEnd.Entities.Roles.ClientUser });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Name" },
                values: new object[] { PDRProvBackEnd.Entities.Roles.Administrador });

            //relación con superadmin
            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RolesName", "UsersId" },
                values: new object[] { PDRProvBackEnd.Entities.Roles.SuperAdmin, this.UserIdAdmin });
            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RolesName", "UsersId" },
                values: new object[] { PDRProvBackEnd.Entities.Roles.Administrador, this.UserIdAdministrador });
            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RolesName", "UsersId" },
                values: new object[] { PDRProvBackEnd.Entities.Roles.ClientUser, this.UserIdClient });

            migrationBuilder.InsertData
                (
                table: "Suppliers",
                columns: new[] {"Id", "Rut", "NameSupplier", "Giro", "NameContact", "PhoneContact","ApprovalDate", "EmailContact", "HasContract","HasOutsourcingService",
                    "HasEthicalManagement", "HasCertification","Disabled","CreatedAt"},
                values: new object[] { this.SupplierId,this.SupplierRut, this.SupplierName, this.Giro, this.NombreContact, this.PhoneContact,DateTime.Now, this.EmailContact ,this.hasContact
                ,this.hasCOutSour,this.hasEthicalMana,this.hasCertification,this.disable,DateTime.Now}
                );

            migrationBuilder.InsertData(
            table: "ReasonContacts",
            columns: new[] { "Id", "Reason", "TypeContact" },
            values: new object[] { this.IdReason, "Insert data", 1 });

            migrationBuilder.InsertData(
            table: "MessageContacts",
            columns: new[] { "Id", "Title", "TypeContact", "IsRead", "ResponseToId", "SendingUserId", "ReasonContactId", "Disabled", "CreatedAt", "UpdatedAt" },
            values: new object[] { this.IdIndiceDatos, this.Title, this.TypeContact, this.IsRead, this.IdIndiceDatos, this.IdUserSending, this.IdReason, false, DateTime.UtcNow, null });

            migrationBuilder.InsertData(
            table: "UserSuppliers",
            columns: new[] { "Id", "UserId", "SupplierId", "Disabled", "CreatedAt", "UpdatedAt" },
            values: new object[] { this.UserSupplierId, this.UserIdClient, this.SupplierId, false, DateTime.UtcNow, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
              table: "Role",
              keyColumn: "Name",
              keyValue: PDRProvBackEnd.Entities.Roles.ClientUser);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Name",
                keyValue: PDRProvBackEnd.Entities.Roles.SuperAdmin);


            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Role");

            //usuario admin
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: this.UserIdAdmin);
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: this.UserIdClient);

            migrationBuilder.DropIndex("IX_Users_Username");
            migrationBuilder.DropIndex("IX_Users_Email");
            migrationBuilder.DropIndex("IX_Users_Disabled");
            migrationBuilder.DropIndex("IX_Users_Expires");
            migrationBuilder.DropIndex("IX_Users_CreatedAt");
            migrationBuilder.DropIndex("IX_Users_UpdatedAt");
            migrationBuilder.DropIndex("IX_RoleUser_RolesName");
            migrationBuilder.DropIndex("IX_Role_Name");

            migrationBuilder.DropIndex("IX_Supplier_Id");
            migrationBuilder.DropIndex("IX_Supplier_Rut");
            migrationBuilder.DropIndex("IX_Supplier_Name");
            migrationBuilder.DropIndex("IX_Supplier_Giro");
            migrationBuilder.DropIndex("IX_Supplier_NameContact");
            migrationBuilder.DropIndex("IX_Supplier_PhoneContact");
            migrationBuilder.DropIndex("IX_Supplier_EmailContact");
            migrationBuilder.DropIndex("IX_Supplier_HasContract");
            migrationBuilder.DropIndex("IX_Supplier_HasOutsourcingService");
            migrationBuilder.DropIndex("IX_Supplier_HasEthicalManagement");
            migrationBuilder.DropIndex("IX_Supplier_HasCertification");
            migrationBuilder.DropIndex("IX_Supplier_Disabled");
            migrationBuilder.DropIndex("IX_Supplier_CreatedAt");

            migrationBuilder.DropIndex("IX_Messagecontact_Title");
            migrationBuilder.DropIndex("IX_Messagecontact_TypeContact");
            migrationBuilder.DropIndex("IX_Messagecontact_IsRead");
            migrationBuilder.DropIndex("IX_Messagecontact_ResponseToId");
            migrationBuilder.DropIndex("IX_Messagecontact_SendingUserId");
            migrationBuilder.DropIndex("IX_Messagecontact_ReasonContactId");

            migrationBuilder.DropIndex("IX_ReasonContact_Id");
            migrationBuilder.DropIndex("IX_ReasonContact_Reason");
            migrationBuilder.DropIndex("IX_ReasonContact_ClassificationContact");
        }
    }
}
