package com.example.forum.entity;

import java.time.LocalDateTime;
import com.example.forum.enums.Role;
import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.PrePersist;
import jakarta.persistence.Table;
import lombok.AccessLevel;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Entity
@Getter
@Table(name = "users")
@NoArgsConstructor(access = AccessLevel.PROTECTED)
public class User {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name = "username", nullable = false, length = 30)
    private String username;

    @Column(name = "photo")
    private String photo;

    @Column(unique = true, nullable = false)
    private String email;

    @Column(name = "password_hash", nullable = false, length = 60)
    private String passwordHash;

    @Column(name = "is_deleted")
    private boolean isDeleted = false;

    @Column(name = "role", nullable = false)
    @Enumerated(EnumType.STRING)
    private Role role;

    // TODO: Create activation method and accept role email

    @Column(name = "is_active")
    private boolean isActive = true;

    @Column(name = "created_at", nullable = false)
    private LocalDateTime createdAt;

    public User(String username, String email, String passwordHash, Role role) {
        if (username == null || username.isBlank())
            throw new IllegalArgumentException("Empty username");
        if (username.length() > 30)
            throw new IllegalArgumentException("Too long username");
        if (email == null || email.isBlank() || !email.contains("@"))
            throw new IllegalArgumentException("Invalid email");
        if (passwordHash == null || passwordHash.length() != 60)
            throw new IllegalArgumentException("Inavalid password hash");
        if (role == null)
            throw new IllegalArgumentException("No role for user");

        this.username = username;
        this.email = email;
        this.passwordHash = passwordHash;
        this.role = role;
        this.createdAt = LocalDateTime.now();
        this.isDeleted = false;
        this.isActive = true;

    }

    // Buisness logic

    /**
     * Update profile method for new username and photo.
     * 
     * @param newUsername
     * @param newPhoto
     */
    public void updateProfile(String newUsername, String newPhoto) {
        if (isDeleted) {
            throw new IllegalStateException("You cant update deleted user");
        }
        if (newUsername != null) {
            if (newUsername.isBlank() || newUsername.length() > 30) {
                throw new IllegalArgumentException("Invalid username");
            }
            this.username = newUsername;
        }
        this.photo = newPhoto;

    }

    @PrePersist
    protected void onCreate() {
        if (createdAt == null)
            createdAt = LocalDateTime.now();
    }

    /**
     * Soft delet method
     */
    public void softDelete() {
        if (isDeleted) {
            return;
        }
        this.isDeleted = true;
        this.isActive = false;
    }

    /**
     * Activation method for NOT deleted users
     * 
     * @throws IllegalStateException if user deleted
     */
    public void activate() {
        if (isDeleted)
            throw new IllegalStateException("Cannot activate deleted user");
        if (isActive)
            return; // already activate
        this.isActive = true;
    }

    /**
     * Deactivation user method
     * 
     * @throws IllegalStateException if user is deleted
     */
    public void deactivate() {
        if (isDeleted) {
            throw new IllegalStateException("You cant deactivate deleted user");
        }
        this.isActive = false;
    }

    /**
     * Checking if he can moderate (for admin and moderator roles)
     * 
     * @return true if user moderator or admin
     */
    public boolean canModerate() {
        return !isDeleted && isActive && (this.role == Role.MODERATOR || this.role == Role.ADMIN);
    }

    /**
     * Checking administrator rights
     * 
     * @return true if user is admin
     */
    public boolean isAdmin() {
        return this.role == Role.ADMIN;
    }

    // TODO: Creadte isAuthorOf(Thread thread) method for check thread author. Need
    // thread entity for complite.

}
