package todss.rest.homereval.repositories;

import todss.rest.homereval.persistence.ApplicationUser;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ApplicationUserRepository extends JpaRepository<ApplicationUser, Long> {
	ApplicationUser findByUsername(String username);
}
