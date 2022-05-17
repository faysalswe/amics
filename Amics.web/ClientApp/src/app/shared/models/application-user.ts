export class ApplicationUser {
  /**
   * Application User View Model.
   */
  constructor(
    public userId: string = "",
    public fullName?: string | null,
    public role: string = "",
    public dB: string =""
  ) {}

  isAuthenticated(): boolean {
    return !!this.userId;
  }
 
  isBasicUser(): boolean {
    return (
      this.role === "Basic" ||
      this.role === "Admin"      
    );
  }

  isAdmin(): boolean {
    return (
      this.role === "Admin"  
    );
  }

  isWebMaster(): boolean {
    return this.role === "WebMaster";
  }
 
}
