namespace EduSource.Contract.Enumarations.MessagesList;

public enum MessagesList
{
    [Message("This email has been registered", "auth_email_exists")]
    AuthEmailExistException,

    [Message("This email has not been registered, please try again", "auth_email_02")]
    AuthEmailNotFoundException,

    [Message("Registration failed, please register again", "auth_register_failure")]
    AuthRegisterFailure,

    [Message("Registration successful, please check email for confirmation", "auth_register_success")]
    AuthRegisterSuccess,

    [Message("Registration time has passed, please register again", "auth_noti_01")]
    AuthRegisterTimeOutException,

    [Message("Account confirmation successful", "auth_noti_04")]
    VerifyEmailSuccess,

    [Message("Registration successful, you can now login", "auth_verify_success")]
    AuthVerifyEmailRegister,

    [Message("Your email is not registered", "auth_not_regist")]
    AuthEmailNotExsitException,

    [Message("This account was registered using another method", "auth_regis_another")]
    AuthAccountRegisteredAnotherMethod,

    [Message("Passwords do not match", "auth_password_not_match")]
    AuthPasswordNotMatchException,

    [Message("Your account has been banned", "account_banned")]
    AccountBanned,

    [Message("Logout successfully", "auth_logout_success")]
    AuthLogoutSuccess,

    [Message("Session has expired, please log in again", "auth_login_expired")]
    AuthRefreshTokenNull,

    [Message("Login Google fail, please try again", "auth_noti_09")]
    AuthLoginGoogleFail,

    [Message("Your OTP does not match", "auth_otp_01")]
    AuthOtpForgotPasswordNotMatchException,

    [Message("Unable to change password, please try again", "auth_forgot_01")]
    AuthErrorChangePasswordException,

    [Message("Please check your email to enter otp", "auth_noti_05")]
    AuthForgotPasswordEmailSuccess,

    [Message("Verify OTP successfully", "auth_noti_06")]
    AuthForgotPasswordOtpSuccess,

    [Message("Your account password has been changed successfully.", "auth_noti_07")]
    AuthForgotPasswordChangeSuccess,

    [Message("This email is already registered with Google", "auth_email_03")]
    AuthGoogleEmailRegisted,

    [Message("Can not find this account!", "account_noti_exception_01")]
    AccountNotFoundException,

    [Message("Can not find any books!", "book_noti_exception_01")]
    BooksNotFoundException,

    [Message("All Books: ", "book_noti_success_01")]
    BookGetAllBooksSuccess,

    [Message("Update information profile successfully", "account_noti_05")]
    AccountUpdateInformationSuccess,

    [Message("Please check email to change", "account_noti_06")]
    AccountUpdateChangeEmail,

    [Message("Email must be different from previous email", "account_email_01")]
    AccountEmailDuplicate,

    [Message("Updated email successfully", "account_noti_07")]
    AccountUpdateEmailSuccess,

    [Message("Account must login by system to change", "account_noti_08")]
    AccountNotLoginUpdate,

    [Message("This email is valid, please wait for another email", "account_email_02")]
    AccountEmailUpdateExit,

    [Message("Please check mail to change password", "account_noti_09")]
    AccountChangePasswordSuccess,

    [Message("Change password successfully", "account_noti_10")]
    ChangePasswordSuccess,

    [Message("Avatar upload successful", "account_noti_03")]
    AccountUploadAvatarSuccess,

    [Message("Get information profile successfully", "account_noti_04")]
    AccountGetInfoProfileSuccess,

    [Message("Can not find any products!", "product_noti_exception_01")]
    ProductsNotFoundException,

    [Message("Can not find this Product!", "product_noti_exception_02")]
    ProductNotFoundException,

    [Message("All Products: ", "product_noti_success_01")]
    ProductGetAllProductsSuccess,

    [Message("All Products Purchased: ", "product_noti_success_02")]
    ProductGetAllProductsPurchasedSuccess,

    [Message("Details of Product: ", "product_noti_success_02")]
    ProductGetDetailsProductSuccess,

    [Message("Details of Product By User: ", "product_noti_success_02")]
    ProductGetDetailsProductByUserSuccess,

    [Message("Create Product successfully", "product_noti_success_03")]
    ProductCreateProductSuccess,

    [Message("Can not find this Book!", "book_noti_exception_01")]
    BookNotFoundException,

    [Message("Product has already added to cart!", "cart_noti_exception_01")]
    CartProductHasAlreadyAddedToCartException,

    [Message("Can not found this Product in cart!", "cart_noti_exception_02")]
    CartProductNotFoundInCartException,

    [Message("There are some products that are not in cart!", "cart_noti_exception_03")]
    CartProductNotInCartException,

    [Message("Can not found any Products in cart!", "cart_noti_exception_03")]
    CartProductsNotFoundInCartException,

    [Message("Add Product to cart successfully", "cart_noti_success_01")]
    CartAddedProductToCartSuccess,

    [Message("Remove Product from cart successfully", "cart_noti_success_02")]
    CartRemovedProductFromCartSuccess,

    [Message("All Products in cart: ", "cart_noti_success_03")]
    CartGetAllProductsFromCartSuccess,

    [Message("Can not find any orders!", "order_noti_exception_01")]
    OrdersNotFoundException,

    [Message("All Orders: ", "order_noti_success_01")]
    OrderGetAllOrdersSuccess,

    [Message("Dashboard Response: ", "order_noti_success_02")]
    OrderGetDashboardSuccess,

}
