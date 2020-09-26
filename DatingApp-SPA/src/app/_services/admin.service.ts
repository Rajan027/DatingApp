import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  getUsersWithRoles() {
    return this.httpClient.get(this.baseUrl + 'admin/usersWithRoles');
  }

  updateUserRoles(user: User, roles: {}) {
    return this.httpClient.post(this.baseUrl + 'admin/editRoles/' + user.userName, roles);
  }

  getPhotosForApproval() {
    return this.httpClient.get(this.baseUrl + 'admin/photosForModeration');
  }

  approvePhoto(photoId: number) {
    return this.httpClient.post(this.baseUrl + 'admin/approvePhoto/' + photoId, {});
  }

  rejectPhoto(photoId: number) {
    return this.httpClient.post(this.baseUrl + 'admin/rejectPhoto/' + photoId, {});
  }
}
