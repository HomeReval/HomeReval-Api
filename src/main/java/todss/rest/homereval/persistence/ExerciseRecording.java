package todss.rest.homereval.persistence;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import java.util.Arrays;

@Entity
@JsonIgnoreProperties({"hibernateLazyInitializer", "handler"})
public class ExerciseRecording {
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private long id;

	private byte[] recording;

    public long getId() {
        return id;
    }

    public byte[] getRecording() {
        return recording;
    }

    public void setRecording(byte[] recording) {
        this.recording = recording;
    }

    protected ExerciseRecording() {}

    @Override
    public String toString() {
        return "ExerciseRecording{" +
                "id=" + id +
                ", recording=" + Arrays.toString(recording) +
                '}';
    }
}
