import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../_services/admin.service';
import { User } from '../../_models/User';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from '../../_modals/roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  imports: [],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit {
  
  private adminService = inject(AdminService);
  modalService = inject(BsModalService);
  users : User[] = [];
  bsModalRef : BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();

  ngOnInit(): void {
    this.getUsersWithRole();
  }

  openRolesModal(user : User){
    const initalState : ModalOptions = {
      class : "modal-lg",
      initialState : {
        username : user.username,
        title : "User roles",
        selectedRoles : [...user.roles],
        availableRoles : ['Admin', 'Moderator', 'Member'],
        users : this.users,
        rolesUpdated : false
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, initalState);
    this.bsModalRef.onHide?.subscribe({
      next : () => {
        if(this.bsModalRef.content && this.bsModalRef.content.rolesUpdated){
          const selectedRoles = this.bsModalRef.content.selectedRoles;
          this.adminService.updateUserRoles(user.username, selectedRoles).subscribe({
            next: roles => {
              user.roles = roles
            }
          })
          
        }
      }
    })
  }

  getUsersWithRole(){
    this.adminService.getUsersWithRole().subscribe({
      next : users => {
        this.users = users;
      }
    })
  }
}
