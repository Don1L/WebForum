package com.example.forum.entity;

import java.time.LocalDateTime;
import com.example.forum.enums.Role;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.PrePersist;
import jakarta.persistence.Table;
import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.NotBlank;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@Entity
@Setter
@Getter
@Table(name = "users")
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class User {

    @jakarta.persistence.Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "username", nullable = false, length = 30)
    private String username;

    @Column(name = "photo", nullable = true)
    private String photo;

    @Email(message = "Некоректный формат email")
    @Column(unique = true, nullable = false)
    @NotBlank(message = "Поле не может быть пустым")
    private String email;

    @Column(name = "password_hash", nullable = false, length = 60)
    private String passwordHash;

    @Column(name = "is_deleted")
    @Builder.Default
    private boolean isDeleted = false;

    @Column(name = "role", nullable = false)
    @Enumerated(EnumType.STRING)
    private Role role;

    // TODO: Create activation method and accept role email

    @Column(name = "is_active")
    @Builder.Default
    private boolean isActive = true;

    @Column(name = "created_at", nullable = false)
    private LocalDateTime createdAt;

    // Buisness logic

    @PrePersist
    protected void onCreate() {
        if (createdAt == null)
            createdAt = LocalDateTime.now();
    }

    /**
     * Soft delet function
     */
    public void delete() {
        if (isDeleted == false)
            this.isDeleted = true;
    }

    /**
     * Ban function for regualr users or moderators
     * 
     * @throws IllegalStateException
     */
    public void ban() {
        if (role != Role.ADMIN)
            this.isActive = false;
        else
            throw new IllegalStateException("Вы не можете забанить админа");
    }

    /**
     * Activation function for NOT deleted users
     * 
     * @throws IllegalStateException
     */
    public void activation() {
        if (isDeleted != true)
            this.isActive = true;
        else
            throw new IllegalStateException("Вы не можете активировать удалённого пользователя");
    }

    /**
     * Checking if he can moderate (for admin and moderator roles)
     * 
     * @return
     */
    public boolean canModerate() {
        return this.role == Role.MODERATOR || this.role == Role.ADMIN;
    }

    /**
     * Checking administrator rights
     * 
     * @return
     */
    public boolean isAdmin() {
        return this.role == Role.ADMIN;
    }

    /**
     * Checking user rights
     * 
     * @return
     */
    public boolean isUser() {
        return this.role == Role.USER;
    }

    /**
     * Checking guest rights
     * 
     * @return
     */
    public boolean isGuest() {
        return this.role == Role.GUEST;
    }

    // TODO: Creadte isAuthorOf(Thread thread) method for check thread author. Need
    // thread entity for complite.

    /**
     * Activation function for guest role (guest != user befor activation)
     * 
     * @throws IllegalStateException
     */
    public void guestActivation() {
        if (role == Role.GUEST && isDeleted != true)
            this.role = Role.USER;
        else
            throw new IllegalStateException("Вы не можете активировать удалённого гостя");
    }

}
