package iac.rest.webshop.persistence;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.List;

@Entity
@JsonIgnoreProperties({"hibernateLazyInitializer", "handler"})
public class Exercise {
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private long id;

	private String name;

	private String description;

    @OneToMany(cascade = CascadeType.ALL, orphanRemoval = true)
    @JoinColumn(name = "exercise_id", foreignKey = @ForeignKey(name="fk_exercise_exerciserecording"))
    private List<ExerciseRecording> exerciseRecordings = new ArrayList<>();

	protected Exercise() {}

    public long getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public List<ExerciseRecording> getExerciseRecordings() {
        return exerciseRecordings;
    }

    public void setExerciseRecordings(List<ExerciseRecording> exerciseRecordings) {
        this.exerciseRecordings = exerciseRecordings;
    }

    @Override
    public String toString() {
        return "Exercise{" +
                "id=" + id +
                ", name='" + name + '\'' +
                ", description='" + description + '\'' +
                '}';
    }
}
